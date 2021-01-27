using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoGraphEdgeModule : ArangoModule, IArangoGraphEdgeModule
    {
        internal ArangoGraphEdgeModule(IArangoContext context) : base(context)
        {
        }

        public async Task<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoEdgeResponse<TR>>(HttpMethod.Get,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}"),
                cancellationToken: cancellationToken);

            return res.Edge;
        }

        public async Task<ArangoEdgeResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<ArangoVoid>>(HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<ArangoVoid>>(HttpMethod.Patch,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<ArangoVoid>>(HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<ArangoVoid>> RemoveAsync<T>(ArangoHandle database, string graph, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<ArangoVoid>>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}"), cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<TR>>(HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<TR>>(HttpMethod.Patch,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<TR>>(HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoEdgeResponse<TR>> RemoveAsync<T, TR>(ArangoHandle database, string graph, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoEdgeResponse<TR>>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}"), cancellationToken: cancellationToken);
        }
    }
}