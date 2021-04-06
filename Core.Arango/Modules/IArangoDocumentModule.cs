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
        ///     Reads a single document
        /// </summary>
        Task<T> GetAsync<T>(ArangoHandle database,
            string collection,
            string key,
            bool throwOnError = true,
            string ifMatch = null,
            string ifNoneMatch = null,
            CancellationToken cancellationToken = default) where T : class;

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
            CancellationToken cancellationToken = default) where T : class;

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
            CancellationToken cancellationToken = default) where T : class;

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
            CancellationToken cancellationToken = default) where T : class;

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
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Removes multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> DeleteManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Removes single document by key
        /// </summary>
        Task<ArangoUpdateResult<TR>> DeleteAsync<TR>(ArangoHandle database, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null, bool? silent = null, string ifMatch = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Note: this API is currently not supported on cluster coordinators.
        /// </summary>
        IAsyncEnumerable<List<T>> ExportAsync<T>(ArangoHandle database, string collection, bool? flush = null,
            int? flushWait = null, int? batchSize = null, int? ttl = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Bulk import
        /// </summary>
        Task ImportAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs, bool complete = true,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Replaces multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> ReplaceManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? returnNew = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Replaces multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<ArangoVoid>>> ReplaceManyAsync<T>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? returnNew = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Replace single document
        /// </summary>
        Task<ArangoUpdateResult<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false, bool? returnOld = null, bool? returnNew = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Replace single document
        /// </summary>
        Task<ArangoUpdateResult<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false, bool? returnOld = null, bool? returnNew = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Updates multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<ArangoVoid>>> UpdateManyAsync<T>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null,
            bool? returnOld = null, bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Updates multiple documents
        /// </summary>
        Task<List<ArangoUpdateResult<TR>>> UpdateManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null,
            bool? returnOld = null, bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Updates single document
        /// </summary>
        Task<ArangoUpdateResult<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null,
            bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     Updates single document
        /// </summary>
        Task<ArangoUpdateResult<TR>> UpdateAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null,
            bool? returnNew = null, bool? silent = null, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default) where T : class;
    }
}