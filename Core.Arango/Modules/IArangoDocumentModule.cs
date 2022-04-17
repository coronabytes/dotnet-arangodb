using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Document module
    /// </summary>
    public interface IArangoDocumentModule
    {
        /// <summary>
        ///     Read a single document
        /// </summary>
        /// <param name="handle">database, transaction, batch handle</param>
        /// <param name="collection">collection</param>
        /// <param name="key">document key</param>
        /// <param name="throwOnError">when false does not throw an exception if document does not exist</param>
        /// <param name="ifMatch">
        ///     The document is returned, if it has the same revision as the given Etag. Otherwise a HTTP 412 is
        ///     returned.
        /// </param>
        /// <param name="ifNoneMatch">
        ///     The document is returned, if it has a different revision than the given Etag. Otherwise an
        ///     HTTP 304 is returned.
        /// </param>
        /// <param name="cancellationToken"></param>
        Task<T> GetAsync<T>(ArangoHandle handle,
            string collection,
            string key,
            bool throwOnError = true,
            string ifMatch = null,
            string ifNoneMatch = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Read multiple documents
        /// </summary>
        /// <param name="handle">database, transaction, batch handle</param>
        /// <param name="collection">collection</param>
        /// <param name="keys">list of keys with optional revision</param>
        /// <param name="ignoreRevs">
        ///     If a search document contains a value for the _rev field, then the document is only returned
        ///     if it has the same revision value
        /// </param>
        /// <param name="cancellationToken"></param>
        Task<List<T>> GetManyAsync<T>(ArangoHandle handle,
            string collection,
            IEnumerable<object> keys,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> CreateManyAsync<T, TR>(ArangoHandle database,
            string collection,
            IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<ArangoVoid>>> CreateManyAsync<T>(ArangoHandle database,
            string collection,
            IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create document
        /// </summary>
        Task<ArangoUpdateResult<TR>> CreateAsync<T, TR>(ArangoHandle database,
            string collection, T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create document
        /// </summary>
        Task<ArangoUpdateResult<ArangoVoid>> CreateAsync<T>(ArangoHandle database,
            string collection, T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> DeleteManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes single document by key
        /// </summary>
        Task<ArangoUpdateResult<TR>> DeleteAsync<TR>(ArangoHandle database, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null, bool? silent = null, string ifMatch = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Export all documents in batches from a collection (Query.ExecuteStreamAsync)
        /// </summary>
        IAsyncEnumerable<List<T>> ExportAsync<T>(ArangoHandle database, string collection, bool? flush = null,
            int? flushWait = null, int? batchSize = null, int? ttl = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Bulk import
        /// </summary>
        Task ImportAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs, bool complete = true,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replaces multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> ReplaceManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? returnNew = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replaces multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<ArangoVoid>>> ReplaceManyAsync<T>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? returnNew = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace single document
        /// </summary>
        Task<ArangoUpdateResult<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false, bool? returnOld = null, bool? returnNew = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace single document
        /// </summary>
        Task<ArangoUpdateResult<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false, bool? returnOld = null, bool? returnNew = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<ArangoVoid>>> UpdateManyAsync<T>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null,
            bool? returnOld = null, bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> UpdateManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null,
            bool? returnOld = null, bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates single document
        /// </summary>
        Task<ArangoUpdateResult<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null,
            bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates single document
        /// </summary>
        Task<ArangoUpdateResult<TR>> UpdateAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null,
            bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default);
    }
}