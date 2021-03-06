using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Graph vertex manipulation
    /// </summary>
    public interface IArangoGraphVertexModule
    {
        /// <summary>
        ///     fetches an existing vertex
        /// </summary>
        Task<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create a new vertex
        /// </summary>
        Task<ArangoVertexResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph, string collection,
            T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create a new vertex
        /// </summary>
        Task<ArangoVertexResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update an existing vertex
        /// </summary>
        Task<ArangoVertexResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update an existing vertex
        /// </summary>
        Task<ArangoVertexResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replaces an existing vertex
        /// </summary>
        Task<ArangoVertexResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replaces an existing vertex
        /// </summary>
        Task<ArangoVertexResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes a vertex from a graph
        /// </summary>
        Task<ArangoVertexResponse<ArangoVoid>> RemoveAsync(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes a vertex from a graph
        /// </summary>
        Task<ArangoVertexResponse<TR>> RemoveAsync<TR>(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);
    }
}