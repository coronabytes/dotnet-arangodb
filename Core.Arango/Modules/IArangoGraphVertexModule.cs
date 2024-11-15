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
        ValueTask<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create a new vertex
        /// </summary>
        ValueTask<ArangoVertexResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph, string collection,
            T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Create a new vertex
        /// </summary>
        ValueTask<ArangoVertexResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update an existing vertex
        /// </summary>
        ValueTask<ArangoVertexResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update an existing vertex
        /// </summary>
        ValueTask<ArangoVertexResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replaces an existing vertex
        /// </summary>
        ValueTask<ArangoVertexResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replaces an existing vertex
        /// </summary>
        ValueTask<ArangoVertexResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes a vertex from a graph
        /// </summary>
        ValueTask<ArangoVertexResponse<ArangoVoid>> RemoveAsync(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Removes a vertex from a graph
        /// </summary>
        ValueTask<ArangoVertexResponse<TR>> RemoveAsync<TR>(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);
    }
}