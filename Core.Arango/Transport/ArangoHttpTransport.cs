using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Transport
{
    public class ArangoHttpTransport : IArangoTransport
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly IArangoConfiguration _configuration;
        private string _auth;
        private DateTime _authValidUntil = DateTime.MinValue;

        public ArangoHttpTransport(IArangoConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<T> SendAsync<T>(HttpMethod m, string url, object body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            if (auth && (_auth == null || _authValidUntil < DateTime.UtcNow.AddMinutes(-10)))
            {
                var authResponse = await SendAsync<JObject>(HttpMethod.Post,
                    "/_open/auth",
                    new
                    {
                        username = _configuration.User,
                        password = _configuration.Password ?? string.Empty
                    }, auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Value<string>("jwt");
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }

            var msg = new HttpRequestMessage(m, _configuration.Server + url);

            msg.Headers.Add(HttpRequestHeader.KeepAlive.ToString(), "true");

            if (auth)
                msg.Headers.Add(HttpRequestHeader.Authorization.ToString(), _auth);

            if (transaction != null)
                msg.Headers.Add("x-arango-trx-id", transaction);

            if (body != null)
            {
                var json = _configuration.Serializer.Serialize(body);
                msg.Content = new StringContent(json, Encoding.UTF8,
                    "application/json");
            }
            else
            {
                msg.Headers.Add(HttpRequestHeader.ContentLength.ToString(), "0");
            }

            var res = await HttpClient.SendAsync(msg, cancellationToken);

            if (!res.IsSuccessStatusCode)
                if (throwOnError)
                    throw new ArangoException(await res.Content.ReadAsStringAsync());
                else return default;

            var content = await res.Content.ReadAsStringAsync();

            if (res.Headers.TryGetValues("X-Arango-Error-Codes", out var errorCodes))
                throw new ArangoException(content);

            if (content == "{}")
                return default;

            return _configuration.Serializer.Deserialize<T>(content);
        }


        public async Task<object> SendAsync(Type type, HttpMethod m, string url, object body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            if (auth && (_auth == null || _authValidUntil < DateTime.UtcNow.AddMinutes(-10)))
            {
                var authResponse = await SendAsync<JObject>(HttpMethod.Post,
                    "/_open/auth",
                    new
                    {
                        username = _configuration.User,
                        password = _configuration.Password ?? string.Empty
                    }, auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Value<string>("jwt");
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }

            var msg = new HttpRequestMessage(m, _configuration.Server + url);

            msg.Headers.Add(HttpRequestHeader.KeepAlive.ToString(), "true");

            if (auth)
                msg.Headers.Add(HttpRequestHeader.Authorization.ToString(), _auth);

            if (transaction != null)
                msg.Headers.Add("x-arango-trx-id", transaction);

            if (body != null)
                msg.Content = new StringContent(_configuration.Serializer.Serialize(body), Encoding.UTF8,
                    "application/json");
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

            return _configuration.Serializer.Deserialize(content, type);
        }
    }
}