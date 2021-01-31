using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoGraphEdgeModule
    {
        Task<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph, string collection,
            T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<ArangoVoid>> RemoveAsync<T>(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);

        Task<ArangoEdgeResponse<TR>> RemoveAsync<T, TR>(ArangoHandle database, string graph, string collection,
            string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default);
    }
}