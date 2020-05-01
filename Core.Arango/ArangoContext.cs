using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    /// <summary>
    ///     Thread-Safe ArangoDB Context
    /// </summary>
    public partial class ArangoContext
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
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
            var builder = new DbConnectionStringBuilder {ConnectionString = cs};
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

            if (string.IsNullOrWhiteSpace(realm))
                throw new ArgumentException("Realm invalid");

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("User invalid");

            Realm = realm + "-";
            Server = server;
            _user = user;
            _password = password;
        }

        public int BatchSize { get; set; } = 500;

        public string Realm { get; }

        public string Server { get; }

        /// <summary>
        /// Callback for query stats
        /// </summary>
        public Action<string, IDictionary<string, object>, JToken> QueryProfile { get; set; }
    }
}