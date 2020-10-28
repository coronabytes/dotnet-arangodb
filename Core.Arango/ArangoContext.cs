using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Core.Arango.Modules.Internal;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    /// <summary>
    ///     Thread-Safe ArangoDB Context
    /// </summary>
    public class ArangoContext : IArangoContext
    {
        public ArangoContext(IArangoConfiguration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            Configuration = config;

            if (string.IsNullOrWhiteSpace(config.Realm))
                Configuration.Realm = string.Empty;
            else
                Configuration.Realm = config.Realm + "-";
        }

        public ArangoContext(string cs, IArangoConfiguration settings = null)
        {
            Configuration = settings ?? new ArangoConfiguration();

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

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("User invalid");

            if (string.IsNullOrWhiteSpace(realm))
                Configuration.Realm = string.Empty;
            else
                Configuration.Realm = realm + "-";

            Configuration.Server = server;
            Configuration.User = user;
            Configuration.Password = password;

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
        public IArangoConfiguration Configuration { get; }

        public async Task<Version> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            var res = await Configuration.Transport.SendAsync<JObject>(HttpMethod.Get,
                "/_db/_system/_api/version",
                cancellationToken: cancellationToken);

            var version = res.Value<string>("version");
            version = Regex.Replace(version, "[^0-9.]", string.Empty);
            return Version.Parse(version);
        }
        public async Task<IReadOnlyCollection<string>> GetEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var res = await Configuration.Transport.SendAsync<JObject>(HttpMethod.Get,
                "/_api/cluster/endpoints", cancellationToken: cancellationToken);

            var endpoints = res.Value<JArray>("endpoints").
                Select(x => x.Value<string>("endpoint")).ToList();

            return new ReadOnlyCollection<string>(endpoints);
        }
    }
}