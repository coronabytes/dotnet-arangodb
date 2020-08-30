using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Modules;
using Core.Arango.Modules.Internal;
using Core.Arango.Protocol;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public interface IArangoContext
    {
        int BatchSize { get; set; }
        string Realm { get; }
        string Server { get; }

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

        Task<object> SendAsync(Type type, HttpMethod m, string url, string body = null, string transaction = null, bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);
        Task<T> SendAsync<T>(HttpMethod m, string url, string body = null, string transaction = null, bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);
    }
}