using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Core.Arango.Modules.Internal;
using Core.Arango.Protocol.Internal;
using Newtonsoft.Json;
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
            Configuration = config ?? throw new ArgumentNullException(nameof(config));
            
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
            Foxx = new ArangoFoxxModule(this);
        }

        public ArangoContext(string cs, IArangoConfiguration settings = null)
        {
            Configuration = settings ?? new ArangoConfiguration();
            Configuration.ConnectionString = cs;

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
            Foxx = new ArangoFoxxModule(this);
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
        public IArangoFoxxModule Foxx { get; }

        private class VersionResponse
        {
            [JsonPropertyName("version")]
            [JsonProperty("version")]
           public string Version { get; set; }
        }

        public async Task<Version> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            var res = await Configuration.Transport.SendAsync<VersionResponse>(HttpMethod.Get,
                "/_db/_system/_api/version",
                cancellationToken: cancellationToken);

            var version = res.Version;
            version = Regex.Replace(version, "[^0-9.]", string.Empty);
            return Version.Parse(version);
        }

        public async Task<IReadOnlyCollection<string>> GetEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var res = await Configuration.Transport.SendAsync<EndpointResponse>(HttpMethod.Get,
                "/_api/cluster/endpoints", cancellationToken: cancellationToken);

            return new ReadOnlyCollection<string>(res.Endpoints.Select(x=>x.Endpoint).ToList());
        }
    }
}