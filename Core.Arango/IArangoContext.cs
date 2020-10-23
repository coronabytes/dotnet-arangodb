using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public interface IArangoContext
    {
        IArangoConfiguration Configuration { get; }

        /// <summary>
        ///     Callback for query stats
        /// </summary>
        Action<string, IDictionary<string, object>, JToken> QueryProfile { get; set; }

        IArangoUserModule User { get; }
        IArangoCollectionModule Collection { get; }
        IArangoGraphModule Graph { get; }
        IArangoTransactionModule Transaction { get; }
        IArangoDocumentModule Document { get; }
        IArangoQueryModule Query { get; }
        IArangoDatabaseModule Database { get; }
        IArangoViewModule View { get; }
        IArangoIndexModule Index { get; }
        IArangoAnalyzerModule Analyzer { get; }
        IArangoFunctionModule Function { get; }

        Task<Version> GetVersionAsync(CancellationToken cancellationToken = default);
    }
}