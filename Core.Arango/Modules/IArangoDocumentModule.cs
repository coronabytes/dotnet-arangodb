using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Document operations
    /// </summary>
    public interface IArangoDocumentModule
    {
        /// <summary>
        ///     reads a single document
        /// </summary>
        Task<T> GetAsync<T>(ArangoHandle database,
            string collection,
            string key,
            bool throwOnError = true,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        ///     creates documents
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
        ///     creates documents
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
        ///     create document
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
        ///     create document
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

        Task<List<ArangoUpdateResult<TR>>> DeleteManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<TR>> DeleteAsync<TR>(ArangoHandle database, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null, bool? silent = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Note: this API is currently not supported on cluster coordinators.
        /// </summary>
        IAsyncEnumerable<List<T>> ExportAsync<T>(ArangoHandle database, string collection, bool? flush = null,
            int? flushWait = null, int? batchSize = null, int? ttl = null,
            CancellationToken cancellationToken = default);

        Task ImportAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs, bool complete = true,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<TR>>> ReplaceManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<ArangoVoid>>> ReplaceManyAsync<T>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? returnOld = null, bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false, bool? returnOld = null, bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false, bool? returnOld = null, bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<ArangoVoid>>> UpdateManyAsync<T>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null,
            bool? returnOld = null, bool? returnNew = null, bool? silent = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<List<ArangoUpdateResult<TR>>> UpdateManyAsync<T, TR>(ArangoHandle database, string collection,
            IEnumerable<T> docs, bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null,
            bool? returnOld = null, bool? returnNew = null, bool? silent = null,
            CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null,
            bool? returnNew = null, bool? silent = null, CancellationToken cancellationToken = default) where T : class;

        Task<ArangoUpdateResult<TR>> UpdateAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null,
            bool? returnNew = null, bool? silent = null, CancellationToken cancellationToken = default) where T : class;
    }
}