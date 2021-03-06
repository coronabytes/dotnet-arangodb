using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Core.Arango.Modules.Internal;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango
{
    /// <inheritdoc/>
    public class ArangoContext : IArangoContext
    {
        /// <summary>
        /// </summary>
        /// <param name="config">configuration</param>
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
            Batch = new ArangoBatchModule(this);
            Backup = new ArangoBackupModule(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="cs">connection string</param>
        /// <param name="settings">settings</param>
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
            Batch = new ArangoBatchModule(this);
            Backup = new ArangoBackupModule(this);
        }

        /// <inheritdoc />
        public IArangoUserModule User { get; }

        /// <inheritdoc />
        public IArangoDatabaseModule Database { get; }

        /// <inheritdoc />
        public IArangoCollectionModule Collection { get; }

        /// <inheritdoc />
        public IArangoViewModule View { get; }

        /// <inheritdoc />
        public IArangoGraphModule Graph { get; }

        /// <inheritdoc />
        public IArangoTransactionModule Transaction { get; }

        /// <inheritdoc />
        public IArangoDocumentModule Document { get; }

        /// <inheritdoc />
        public IArangoQueryModule Query { get; }

        /// <inheritdoc />
        public IArangoIndexModule Index { get; }

        /// <inheritdoc />
        public IArangoAnalyzerModule Analyzer { get; }

        /// <inheritdoc />
        public IArangoFunctionModule Function { get; }

        /// <inheritdoc />
        public IArangoConfiguration Configuration { get; }

        /// <inheritdoc />
        public IArangoFoxxModule Foxx { get; }

        /// <inheritdoc />
        public IArangoBackupModule Backup { get; }

        /// <inheritdoc />
        public IArangoBatchModule Batch { get; }

        /// <inheritdoc />
        public async Task<ArangoVersion> GetVersionAsync(CancellationToken cancellationToken = default)
        {
            var res = await Configuration.Transport.SendAsync<ArangoVersion>(HttpMethod.Get,
                "/_db/_system/_api/version",
                cancellationToken: cancellationToken);

            var clean = Regex.Replace(res.Version, "[^0-9.]", string.Empty);

            res.SemanticVersion = Version.Parse(clean);
            return res;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<string>> GetEndpointsAsync(CancellationToken cancellationToken = default)
        {
            var res = await Configuration.Transport.SendAsync<EndpointResponse>(HttpMethod.Get,
                "/_api/cluster/endpoints", cancellationToken: cancellationToken);

            return new ReadOnlyCollection<string>(res.Endpoints.Select(x => x.Endpoint).ToList());
        }
    }
}