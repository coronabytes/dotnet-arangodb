using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Core.Arango.Linq.Attributes;
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
        private HttpClient _httpClient;

        /// <summary>
        /// </summary>
        public ArangoConfiguration()
        {
            BatchSize = 500;
            Serializer = new ArangoNewtonsoftSerializer(new ArangoNewtonsoftDefaultContractResolver());
            Transport = new ArangoHttpTransport(this);
            ResolveCollection = type =>
            {
                var attr = type.GetCustomAttribute<CollectionPropertyAttribute>();

                if (attr != null)
                    return attr.CollectionName;

                return type.Name;
            };
            ResolveProperty = (type, name) =>
            {
                switch (name)
                {
                    case "Key":
                        return "_key";
                    case "Id":
                        return "_id";
                    case "Revision":
                        return "_rev";
                    case "From":
                        return "_from";
                    case "To":
                        return "_to";
                }

                // TODO: camelCase
                return name;
            };
            ResolveGroupBy = s => s;
        }
        
        /// <inheritdoc />
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
                builder.TryGetValue("AllowDirtyRead", out var dr);
                builder.TryGetValue("Endpoints", out var eps);

                var server = s as string;
                var user = u as string ?? uid as string;
                var password = p as string;
                var realm = r as string;
                var allowDirtyRead = dr as string;
                var endpoints = dr as string;

                if (string.IsNullOrWhiteSpace(server))
                    throw new ArgumentException("Server invalid");

                Realm = realm;
                Server = server;
                User = user;
                Password = password;

                Endpoints = endpoints?.SplitAndRemoveEmptyEntries(',').ToList();

                if (allowDirtyRead?.Equals("true", StringComparison.InvariantCultureIgnoreCase) == true)
                    AllowDirtyRead = true;
            }
        }

        /// <inheritdoc />
        public string Realm { get; set; }

        /// <inheritdoc />
        public string Server { get; set; }

        /// <inheritdoc />
        public string User { get; set; }

        /// <inheritdoc />
        public string Password { get; set; }

        /// <inheritdoc />
        public int BatchSize { get; set; }

        /// <inheritdoc />
        public IArangoSerializer Serializer { get; set; }

        /// <inheritdoc />
        public IArangoTransport Transport { get; set; }

        /// <inheritdoc />
        public Action<string, IDictionary<string, object>, ArangoQueryStatistic> QueryProfile { get; set; }

        /// <inheritdoc />
        public HttpClient HttpClient
        {
            get => _httpClient;
            set => _httpClient = value ?? new HttpClient();
        }

        /// <inheritdoc />
        public bool AllowDirtyRead { get; set; }

        /// <inheritdoc />
        public IReadOnlyList<string> Endpoints { get; set; }
        /// <inheritdoc />
        public Func<Type, string,  string> ResolveProperty { get; set; }
        /// <inheritdoc />
        public Func<Type, string> ResolveCollection { get; set; }
        /// <inheritdoc />
        public Func<string, string> ResolveGroupBy { get; set; }
    }
}