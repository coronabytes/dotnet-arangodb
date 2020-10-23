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
        private string _auth;
        private DateTime _authValidUntil = DateTime.MinValue;
        private IArangoContext _context;

        public void Initialize(IArangoContext context)
        {
            _context = context;
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
                var authResponse = await SendAsync<JObject>(HttpMethod.Post,
                    $"{_context.Configuration.Server}/_open/auth",
                    _context.Configuration.Serializer.Serialize(new
                    {
                        username = _context.Configuration.User,
                        password = _context.Configuration.Password ?? string.Empty
                    }), auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Value<string>("jwt");
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }

            var msg = new HttpRequestMessage(m, url);

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

            if (res.Headers.TryGetValues("X-Arango-Error-Codes", out var errorCodes))
                throw new ArangoException(content);

            if (content == "{}")
                return default;

            return _context.Configuration.Serializer.Deserialize<T>(content);
        }


        public async Task<object> SendAsync(Type type, HttpMethod m, string url, string body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            if (auth && (_auth == null || _authValidUntil < DateTime.UtcNow.AddMinutes(-10)))
            {
                var authResponse = await SendAsync<JObject>(HttpMethod.Post,
                    $"{_context.Configuration.Server}/_open/auth",
                    _context.Configuration.Serializer.Serialize(new
                    {
                        username = _context.Configuration.User,
                        password = _context.Configuration.Password ?? string.Empty
                    }), auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Value<string>("jwt");
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }

            var msg = new HttpRequestMessage(m, url);

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

            return _context.Configuration.Serializer.Deserialize(content, type);
        }
    }
}