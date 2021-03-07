using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;
using Newtonsoft.Json;

namespace Core.Arango.Transport
{
    /// <summary>
    ///   Arango HTTP 1.1/2.0 Transport Implementation
    /// </summary>
    public class ArangoHttpTransport : IArangoTransport
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private readonly IArangoConfiguration _configuration;
        private string _auth;
        private DateTime _authValidUntil = DateTime.MinValue;

        /// <summary>
        ///   Arango HTTP 1.1/2.0 Transport Implementation
        /// </summary>
        public ArangoHttpTransport(IArangoConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc/>
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
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    var error = _configuration.Serializer.Deserialize<ErrorResponse>(errorContent);
                    throw new ArangoException(errorContent, error.ErrorMessage,
                        (HttpStatusCode) error.Code, (ArangoErrorCode) error.ErrorNum);
                }
                else
                {
                    return default;
                }

            var content = await res.Content.ReadAsStringAsync();

            if (res.Headers.Contains("X-Arango-Error-Codes"))
            {
                var errors = _configuration.Serializer.Deserialize<IEnumerable<ErrorResponse>>(content)
                    .Select(error => new ArangoError(error.ErrorMessage, (ArangoErrorCode) error.ErrorNum));
                throw new ArangoException(content, errors);
            }

            if (content == "{}" || string.IsNullOrWhiteSpace(content))
                return default;

            return _configuration.Serializer.Deserialize<T>(content);
        }

        /// <inheritdoc/>
        public Task<T> WriteBatchAsync<T>(ArangoHandle handle, HttpMethod m, string url, object body = null)
        {
            var tcs = new TaskCompletionSource<T>();

            handle.Batches.Add(new ArangoBatch
            {
                ContentId = Guid.NewGuid(),
                Request = $"{m.Method} {url} HTTP/1.1\r\n" + (body != null ? "\r\n" + _configuration.Serializer.Serialize(body)+ "\r\n" : string.Empty),
                Complete = s =>
                {
                    try
                    {
                        tcs.SetResult(_configuration.Serializer.Deserialize<T>(s));
                    }
                    catch (Exception e)
                    {
                        tcs.SetException(e);
                    }
                },
                Cancel = () => tcs.SetCanceled(),
                Fail = exception => tcs.SetException(exception)
            });

            return tcs.Task;
        }

        /// <inheritdoc/>
        public async Task<HttpContent> SendContentAsync(HttpMethod m, string url, HttpContent body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default)
        {
            await Authenticate(auth, cancellationToken);

            var msg = new HttpRequestMessage(m, _configuration.Server + url);

            msg.Headers.Add(HttpRequestHeader.KeepAlive.ToString(), "true");

            if (auth)
                msg.Headers.Add(HttpRequestHeader.Authorization.ToString(), _auth);

            if (transaction != null)
                msg.Headers.Add("x-arango-trx-id", transaction);

            msg.Content = body;

            var res = await HttpClient.SendAsync(msg, cancellationToken);

            if (!res.IsSuccessStatusCode)
                if (throwOnError)
                {
                    var errorContent = await res.Content.ReadAsStringAsync();
                    var error = _configuration.Serializer.Deserialize<ErrorResponse>(errorContent);
                    throw new ArangoException(errorContent, error.ErrorMessage,
                        (HttpStatusCode)error.Code, (ArangoErrorCode)error.ErrorNum);
                }

            return res.Content;
        }
        
        /// <inheritdoc/>
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

            if (content == "{}" || string.IsNullOrWhiteSpace(content))
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
    }
}