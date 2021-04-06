using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Serialization;

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
        protected string ApiPath(ArangoHandle handle, string path, IDictionary<string, string> parameter = null)
        {
            var req = $"/_db/{RealmPrefix(handle)}/_api/{path}";

            if (parameter?.Any() == true)
                req = AddQueryString(req, parameter);

            return req;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string FoxxPath(ArangoHandle handle, string path, IDictionary<string, string> parameter = null)
        {
            var req = $"/_db/{RealmPrefix(handle)}{path}";

            if (parameter?.Any() == true)
                req = AddQueryString(req, parameter);

            return req;
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
        public Task<T> SendAsync<T>(ArangoHandle handle, HttpMethod m, 
            string url, object body = null, 
            bool throwOnError = true, bool auth = true, IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
        {
            if (handle == null)
                return Context.Configuration.Transport.SendAsync<T>(m, url, body, null, throwOnError, auth,
                    headers, cancellationToken);

            if (handle.Batches != null)
                return Context.Configuration.Transport.WriteBatchAsync<T>(handle, m, url, body);

            return Context.Configuration.Transport.SendAsync<T>(m, url, body, handle.Transaction, throwOnError, auth,
                headers,cancellationToken);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<object> SendAsync(Type type, HttpMethod m, string url, object body = null,
            string transaction = null, bool throwOnError = true, bool auth = true, IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default)
        {
            return Context.Configuration.Transport.SendAsync(type, m, url, body, transaction, throwOnError, auth,
                headers, cancellationToken);
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
            var formatter = new AqlQueryFormatter();
            var queryExp = query.ToString(formatter);
            parameter = formatter.Context.Parameters;

            return queryExp;
        }

        protected enum QueryParameterType
        {
            Regular,
            Collection
        }

        protected class QueryFormattingContext
        {
            private readonly IDictionary<(object value, QueryParameterType type), string> _paramsMap =
                new Dictionary<(object obj, QueryParameterType type), string>();

            private int _counter;

            public IDictionary<string, object> Parameters =>
                _paramsMap.ToDictionary(x => x.Value[1..], x => x.Key.value);

            public string Register(QueryParameterType type, object value)
            {
                if (!_paramsMap.TryGetValue((value, type), out var paramName))
                {
                    switch (type)
                    {
                        case QueryParameterType.Regular:
                            paramName = $"@P{++_counter}";
                            break;
                        case QueryParameterType.Collection:
                            paramName = $"@@C{++_counter}";
                            break;
                        default: throw new ArgumentException($"Unsupported parameter type: {type:G}", nameof(type));
                    }

                    _paramsMap.Add((value, type), paramName);
                }

                return paramName;
            }
        }

        protected class AqlQueryFormatter : IFormatProvider, ICustomFormatter
        {
            public QueryFormattingContext Context { get; } = new QueryFormattingContext();

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                string RegisterDefault()
                {
                    var type = format switch
                    {
                        null => QueryParameterType.Regular,
                        "C" => QueryParameterType.Collection,
                        "c" => QueryParameterType.Collection,
                        "@" => QueryParameterType.Collection,
                        _ => throw new FormatException($"Unsupported format: {format}")
                    };

                    return Context.Register(type, arg);
                }

                return arg switch
                {
                    FormattableString formattable => formattable.ToString(this),
                    IArangoFormattable formattable => formattable.ToString(format, this),
                    _ => RegisterDefault()
                };
            }

            public object GetFormat(Type formatType)
            {
                return formatType == typeof(ICustomFormatter) ? this : null;
            }
        }
    }
}