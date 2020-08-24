using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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

        Task<List<ArangoUpdateResult<TR>>> ReplaceDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<JObject>>> ReplaceDocumentsAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<TR>> ReplaceDocumentAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<JObject>> ReplaceDocumentAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<JObject>>> UpdateDocumentsAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<TR>>> UpdateDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<JObject>> UpdateDocumentAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<TR>> UpdateDocumentAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="docs"></param>
        /// <param name="waitForSync"></param>
        /// <param name="silent"></param>
        /// <param name="overwrite">In bulk mode truncates collection!</param>
        /// <param name="bulk">Optimized insert</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            bool bulk = false,
            CancellationToken cancellationToken = default) where T : class;

        Task<T> CreateDocumentAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<TR>>> DeleteDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<TR>> DeleteDocumentAsync<TR>(ArangoHandle database, string collection,
            string key,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? silent = null,
            CancellationToken cancellationToken = default);

        Task<bool> CreateDatabaseAsync(ArangoHandle name, CancellationToken cancellationToken = default);
        Task<List<string>> ListDatabasesAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistDatabaseAsync(ArangoHandle name, CancellationToken cancellationToken = default);
        Task DropDatabaseAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        Task EnsureIndexAsync(ArangoHandle database, string collection, ArangoIndex request,
            CancellationToken cancellationToken = default);

        Task CreateViewAsync(ArangoHandle database, ArangoView view,
            CancellationToken cancellationToken = default);

        Task<List<string>> ListViewsAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        Task DropViewAsync(ArangoHandle database,
            string name,
            CancellationToken cancellationToken = default);

        Task DropViewsAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        Task<List<string>> ListGraphAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        Task CreateGraphAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default);

        Task DropGraphAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default);

        Task CreateCollectionAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default);

        Task CreateCollectionAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default);

        Task TruncateCollectionAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        Task<List<string>> ListCollectionsAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Ignores primary and edge indices
        /// </summary>
        Task<List<string>> ListIndicesAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        Task DropIndexAsync(ArangoHandle database, string index,
            CancellationToken cancellationToken = default);

        Task<Version> GetVersionAsync(CancellationToken cancellationToken = default);

        Task DropCollectionAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops all user created indices over all collections in database
        /// </summary>
        Task DropIndicesAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        Task<List<ArangoAnalyzer>> ListAnalyzersAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        Task CreateAnalyzerAsync(ArangoHandle database,
            ArangoAnalyzer analyzer,
            CancellationToken cancellationToken = default);

        Task DeleteAnalyzerAsync(ArangoHandle database,
            string analyzer, bool force = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     AQL filter expression with x as iterator
        /// </summary>
        Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, int limit = 1000, CancellationToken cancellationToken = default) where T : new();

        Task<T> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, CancellationToken cancellationToken = default) where T : new();

        Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, FormattableString query,
            bool? cache = null, CancellationToken cancellationToken = default)
            where T : new();

        Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
            where T : new();

        /// <summary>
        ///     For Linq Provider
        /// </summary>
        Task<object> QueryAsync(Type type, bool isEnumerable, ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Note: this API is currently not supported on cluster coordinators.
        /// </summary>
        IAsyncEnumerable<List<JObject>> ExportAsync(ArangoHandle database,
            string collection, bool? flush = null, int? flushWait = null, int? batchSize = null, int? ttl = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Prefix database name with realm + urlencode
        /// </summary>
        string DbName(string name);

        /// <summary>
        ///     HTTP request abstraction
        /// </summary>
        Task<T> SendAsync<T>(HttpMethod m, string url, string body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default);

        Task<object> SendAsync(Type type, HttpMethod m, string url, string body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default);

        Task<JObject> ExecuteTransactionAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default);

        Task<ArangoHandle> BeginTransactionAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default);

        Task CommitTransactionAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        Task AbortTransactionAsync(ArangoHandle database, CancellationToken cancellationToken = default);
    }
}