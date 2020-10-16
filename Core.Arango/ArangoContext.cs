using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Core.Arango.Modules.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    /// <summary>
    ///     Thread-Safe ArangoDB Context
    /// </summary>
    public class ArangoContext : IArangoContext
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        internal static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new ArangoContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        private readonly string _password;
        private readonly string _user;
        private string _auth;
        private DateTime _authValidUntil = DateTime.MinValue;

        public ArangoContext(string cs)
        {
            User = new ArangoUserModule(this);
            Collection = new ArangoCollectionModule(this);
            View = new ArangoViewModule(this);
            Database = new ArangoDatabaseModule(this);
            Graph = new ArangoGraphModule(this);
            Transaction = new ArangoTransactionModule(this);
            Document = new ArangoDocumentModule(this);
            Query = new ArangoQueryModule(this);
            Index = new ArangoIndexModule(this);
            Analyzer = new ArangoAnalyzerModule(this);
            Function = new ArangoFunctionModule(this);

            var builder = new DbConnectionStringBuilder { ConnectionString = cs };
            builder.TryGetValue("Server", out var s);
            builder.TryGetValue("Realm", out var r);
            builder.TryGetValue("User ID", out var uid);
            builder.TryGetValue("User", out var u);
            builder.TryGetValue("Password", out var p);

            var server = s as string;
            var user = u as string ?? uid as string;
            var password = p as string;
            var realm = r as string;

            if (string.IsNullOrWhiteSpace(server))
                throw new ArgumentException("Server invalid");

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("User invalid");

            if (string.IsNullOrWhiteSpace(realm))
                Realm = string.Empty;
            else
                Realm = realm + "-";

            Server = server;
            _user = user;
            _password = password;
        }

        public IArangoUserModule User { get; }

        public IArangoDatabaseModule Database { get; }
        public IArangoCollectionModule Collection { get; }
        public IArangoViewModule View { get; }
        public IArangoGraphModule Graph { get; }
        public IArangoTransactionModule Transaction { get; }
        public IArangoDocumentModule Document { get; }
        public IArangoQueryModule Query { get; }
        public IArangoIndexModule Index { get; }
        public IArangoAnalyzerModule Analyzer { get; }
        public IArangoFunctionModule Function { get; }

        public int BatchSize { get; set; } = 500;

        public string Realm { get; }
        public string Server { get; }

        /// <summary>
        ///     Callback for query stats
        /// </summary>
        public Action<string, IDictionary<string, object>, JToken> QueryProfile { get; set; }

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

            Version version = HttpVersion.Version11;

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

        public async Task<Version> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get, $"{Server}/_db/_system/_api/version",
                cancellationToken: cancellationToken);
            return Version.Parse(res.Value<string>("version"));
        }
    }
}