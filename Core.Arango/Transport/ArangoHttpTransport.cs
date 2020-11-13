using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        private class AuthRequest
        {
            [JsonProperty("username")]
            [JsonPropertyName("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        private class AuthResponse
        {
            [JsonProperty("jwt")]
            [JsonPropertyName("jwt")]
            public string Jwt { get; set; }
        }

        public async Task<T> SendAsync<T>(HttpMethod m, string url, object body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            await Authenticate(auth, cancellationToken);

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
            await Authenticate(auth, cancellationToken);

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

        private async Task Authenticate(bool auth, CancellationToken cancellationToken)
        {
            if (auth && (_auth == null || _authValidUntil < DateTime.UtcNow.AddMinutes(-10)))
            {
                var authResponse = await SendAsync<AuthResponse>(HttpMethod.Post,
                    "/_open/auth",
                    new AuthRequest
                    {
                        Username = _configuration.User,
                        Password = _configuration.Password ?? string.Empty
                    }, auth: false, cancellationToken: cancellationToken);

                var jwt = authResponse.Jwt;
                var token = new JwtSecurityToken(jwt.Replace("=", ""));
                _auth = $"Bearer {jwt}";
                _authValidUntil = token.ValidTo;
            }
        }
    }
}