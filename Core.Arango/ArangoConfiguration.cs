using System;
using System.Collections.Generic;
using System.Data.Common;
using Core.Arango.Protocol;
using Core.Arango.Serialization;
using Core.Arango.Serialization.Newtonsoft;
using Core.Arango.Transport;

namespace Core.Arango
{
    public class ArangoConfiguration : IArangoConfiguration
    {
        private string _connectionString;

        public ArangoConfiguration()
        {
            BatchSize = 500;
            Serializer = new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
            Transport = new ArangoHttpTransport(this);
        }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                var builder = new DbConnectionStringBuilder { ConnectionString = value };
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

        public string Realm { get; set; }
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public int BatchSize { get; set; }
        public IArangoSerializer Serializer { get; set; }
        public IArangoTransport Transport { get; set; }
        public Action<string, IDictionary<string, object>, ArangoQueryStatistic> QueryProfile { get; set; }
    }
}