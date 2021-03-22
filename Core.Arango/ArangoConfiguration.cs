using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.Http;
using Core.Arango.Protocol;
using Core.Arango.Serialization;
using Core.Arango.Serialization.Newtonsoft;
using Core.Arango.Transport;

namespace Core.Arango
{
    /// <summary>
    ///     Driver configuration.
    /// </summary>
    public class ArangoConfiguration : IArangoConfiguration
    {
        private string _connectionString;

        /// <summary>
        /// </summary>
        public ArangoConfiguration()
        {
            BatchSize = 500;
            Serializer = new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
            Transport = new ArangoHttpTransport(this);
        }

        
        /// <inheritdoc/>
        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                var builder = new DbConnectionStringBuilder {ConnectionString = value};
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

                Realm = realm;
                Server = server;
                User = user;
                Password = password;
            }
        }

        /// <inheritdoc/>
        public string Realm { get; set; }

        /// <inheritdoc/>
        public string Server { get; set; }

        /// <inheritdoc/>
        public string User { get; set; }

        /// <inheritdoc/>
        public string Password { get; set; }

        /// <inheritdoc/>
        public int BatchSize { get; set; }

        /// <inheritdoc/>
        public IArangoSerializer Serializer { get; set; }

        /// <inheritdoc/>
        public IArangoTransport Transport { get; set; }

        /// <inheritdoc/>
        public Action<string, IDictionary<string, object>, ArangoQueryStatistic> QueryProfile { get; set; }

        /// <inheritdoc/>
        public HttpClient HttpClient { get; set; }
    }
}