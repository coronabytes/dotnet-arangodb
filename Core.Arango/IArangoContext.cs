using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Core.Arango.Protocol;

namespace Core.Arango
{
    /// <summary>
    ///     Arango connection to server or cluster
    /// </summary>
    public interface IArangoContext
    {
        /// <summary>
        ///     Configuration
        /// </summary>
        IArangoConfiguration Configuration { get; }

        /// <summary>
        ///     User management module
        /// </summary>
        IArangoUserModule User { get; }

        /// <summary>
        ///     Collection management module
        /// </summary>
        IArangoCollectionModule Collection { get; }

        /// <summary>
        ///     Graph management and vertex/edge manipulation module
        /// </summary>
        IArangoGraphModule Graph { get; }

        /// <summary>
        ///     Stream and JavaScript transaction module
        /// </summary>
        IArangoTransactionModule Transaction { get; }

        /// <summary>
        ///     Document module
        /// </summary>
        IArangoDocumentModule Document { get; }

        /// <summary>
        ///     Query (cursor) module
        /// </summary>
        IArangoQueryModule Query { get; }

        /// <summary>
        ///     Database management module
        /// </summary>
        IArangoDatabaseModule Database { get; }

        /// <summary>
        ///     View (ArangoSearch) management module
        /// </summary>
        IArangoViewModule View { get; }

        /// <summary>
        ///     Index management module
        /// </summary>
        IArangoIndexModule Index { get; }

        /// <summary>
        ///     Analyzer (ArangoSearch) management module
        /// </summary>
        IArangoAnalyzerModule Analyzer { get; }

        /// <summary>
        ///     Custom functions management module
        /// </summary>
        IArangoFunctionModule Function { get; }

        /// <summary>
        ///     Foxx services module
        /// </summary>
        IArangoFoxxModule Foxx { get; }

        /// <summary>
        ///     HotBackup module (enterprise only)
        /// </summary>
        IArangoBackupModule Backup { get; }

        /// <summary>
        ///  Pregel Module
        /// </summary>
        IArangoPregelModule Pregel { get; }

        /// <summary>
        ///     Get Arango server version and license
        /// </summary>
        ValueTask<ArangoVersion> GetVersionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get Arango cluster endpoints
        /// </summary>
        ValueTask<IReadOnlyCollection<string>> GetEndpointsAsync(CancellationToken cancellationToken = default);
    }
}