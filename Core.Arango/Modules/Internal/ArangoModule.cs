using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules.Internal
{
    internal abstract class ArangoModule
    {
        protected readonly IArangoContext Context;

        protected ArangoModule(IArangoContext context)
        {
            Context = context;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string RealmPrefix(string name)
        {
            if (name == "_system")
                return "_system";

            var realm = string.IsNullOrWhiteSpace(Context.Configuration.Realm)
                ? string.Empty
                : Context.Configuration.Realm + "-";

            return UrlEncode(realm + name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string ApiPath(ArangoHandle handle, string path)
        {
            return $"/_db/{RealmPrefix(handle)}/_api/{path}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string ApiPath(string path)
        {
            return $"/_api/{path}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string UrlEncode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<T> SendAsync<T>(HttpMethod m, string url, object body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            return Context.Configuration.Transport.SendAsync<T>(m, url, body, transaction, throwOnError, auth,
                cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<object> SendAsync(Type type, HttpMethod m, string url, object body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            return Context.Configuration.Transport.SendAsync(type, m, url, body, transaction, throwOnError, auth,
                cancellationToken);
        }

        public string AddQueryString(string uri,
            IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            if (queryString == null)
                throw new ArgumentNullException(nameof(queryString));

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = "";

            if (anchorIndex != -1)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = queryIndex != -1;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);
            foreach (var parameter in queryString)
            {
                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));
                hasQuery = true;
            }

            sb.Append(anchorText);
            return sb.ToString();
        }

        public string Parameterize(FormattableString query, out IDictionary<string, object> parameter)
        {
            var formatter = new AQLQueryFormatter();
            var queryExp = query.ToString(formatter);
            parameter = formatter.Context.Parameters;

            return queryExp;
        }

        protected enum QueryParameterType
        {
            Regular,
            Collection,
        }

        protected class QueryFormattingContext
        {
            private int Counter = 0;
            private readonly IDictionary<(object value, QueryParameterType type), string> ParamsMap = new Dictionary<(object obj, QueryParameterType type), string>();

            public IDictionary<string, object> Parameters => ParamsMap.ToDictionary(x => x.Value.Substring(1), x => x.Key.value);

            public string Register(QueryParameterType type, object value)
            {
                if (!ParamsMap.TryGetValue((value, type), out var paramName))
                {
                    switch (type)
                    {
                        case QueryParameterType.Regular:
                            paramName = $"@P{++Counter}";
                            break;
                        case QueryParameterType.Collection:
                            paramName = $"@@C{++Counter}";
                            break;
                        default: throw new ArgumentException($"Unsupported parameter type: {type:G}", nameof(type));
                    }
                    ParamsMap.Add((value, type), paramName);
                }

                return paramName;
            }
        }

        protected class AQLQueryFormatter : IFormatProvider, ICustomFormatter
        {
            public QueryFormattingContext Context { get; } = new QueryFormattingContext();

            public object GetFormat(Type formatType)
            {
                if (formatType == typeof(ICustomFormatter))
                    return this;
                else
                    return null;
            }

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                if (arg is IFormattable formattable)
                    return formattable.ToString(format, this);

                QueryParameterType type;

                switch (format)
                {
                    case null:
                    case "":
                        type = QueryParameterType.Regular;
                        break;
                    case "C":
                    case "c":
                    case "@":
                        type = QueryParameterType.Collection;
                        break;
                    default:
                        throw new FormatException($"Unsupported format: {format}");
                }

                return Context.Register(type, arg);
            }
        }
    }
}