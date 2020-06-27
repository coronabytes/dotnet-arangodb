using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        /// <summary>
        ///     Prefix database name with realm + urlencode
        /// </summary>
        public string DbName(string name)
        {
            return UrlEncoder.Default.Encode(Realm + name);
        }

        /// <summary>
        ///     Adds query parameters to url
        /// </summary>
        public static string AddQueryString(
            string uri,
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

        /// <summary>
        ///     HTTP request abstraction
        /// </summary>
        public async Task<T> SendAsync<T>(HttpMethod m, string url, string body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            if (auth && (_auth == null || _authValidUntil < DateTime.UtcNow.AddMinutes(-10)))
            {
                var authResponse = await SendAsync<JObject>(HttpMethod.Post, $"{Server}/_open/auth",
                    JsonConvert.SerializeObject(new
                    {
                        username = _user,
                        password = _password ?? string.Empty
                    }, JsonSerializerSettings), auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Value<string>("jwt");
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }

            var msg = new HttpRequestMessage(m, url)
            {
                Version = HttpVersion.Version11
            };

            msg.Headers.Add(HttpRequestHeader.KeepAlive.ToString(), "true");

            if (auth)
                msg.Headers.Add(HttpRequestHeader.Authorization.ToString(), _auth);

            if (transaction != null)
                msg.Headers.Add("x-arango-trx-id", transaction);

            if (body != null)
                msg.Content = new StringContent(body, Encoding.UTF8, "application/json");
            else
                msg.Headers.Add(HttpRequestHeader.ContentLength.ToString(), "0");

            var res = await HttpClient.SendAsync(msg, cancellationToken);

            if (!res.IsSuccessStatusCode)
                if (throwOnError)
                    throw new ArangoException(await res.Content.ReadAsStringAsync());
                else return default;

            var content = await res.Content.ReadAsStringAsync();

            if (content == "{}")
                return default;

            return JsonConvert.DeserializeObject<T>(content, JsonSerializerSettings);
        }

        public async Task<object> SendAsync(Type type, HttpMethod m, string url, string body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            if (auth && (_auth == null || _authValidUntil < DateTime.UtcNow.AddMinutes(-10)))
            {
                var authResponse = await SendAsync<JObject>(HttpMethod.Post, $"{Server}/_open/auth",
                    JsonConvert.SerializeObject(new
                    {
                        username = _user,
                        password = _password ?? string.Empty
                    }, JsonSerializerSettings), auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Value<string>("jwt");
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }

            var msg = new HttpRequestMessage(m, url)
            {
                Version = HttpVersion.Version11
            };

            msg.Headers.Add(HttpRequestHeader.KeepAlive.ToString(), "true");

            if (auth)
                msg.Headers.Add(HttpRequestHeader.Authorization.ToString(), _auth);

            if (transaction != null)
                msg.Headers.Add("x-arango-trx-id", transaction);

            if (body != null)
                msg.Content = new StringContent(body, Encoding.UTF8, "application/json");
            else
                msg.Headers.Add(HttpRequestHeader.ContentLength.ToString(), "0");

            var res = await HttpClient.SendAsync(msg, cancellationToken);

            if (!res.IsSuccessStatusCode)
                if (throwOnError)
                    throw new ArgumentException(await res.Content.ReadAsStringAsync());
                else return default;

            var content = await res.Content.ReadAsStringAsync();

            if (content == "{}")
                return default;

            return JsonConvert.DeserializeObject(content, type, JsonSerializerSettings);
        }

        /// <summary>
        ///     Replaces parameters in interpolated string with placeholders @P1, @P2, ... and produces dictionary with P1=V1,
        ///     P2=V2, ...
        /// </summary>
        /// <remarks>
        ///     identical objects (references) are mapped to the same parameter
        /// </remarks>
        public static string Parameterize(FormattableString query, out Dictionary<string, object> parameter)
        {
            var i = 0;

            var set = new Dictionary<object, string>();
            var nullParam = string.Empty;

            var args = query.GetArguments().Select(x =>
            {
                if (x == null)
                {
                    if (string.IsNullOrEmpty(nullParam))
                        nullParam = $"@P{++i}";

                    return nullParam;
                }

                if (set.TryGetValue(x, out var p))
                    return (object) p;

                p = $"@P{++i}";

                set.Add(x, p);

                return (object) p;
            }).ToArray();

            var queryExp = string.Format(query.Format, args);

            var res = set.ToDictionary(x => x.Value.Substring(1), x => x.Key);

            if (!string.IsNullOrEmpty(nullParam))
                res.Add(nullParam.Substring(1), null);

            parameter = res;

            return queryExp;
        }
    }
}