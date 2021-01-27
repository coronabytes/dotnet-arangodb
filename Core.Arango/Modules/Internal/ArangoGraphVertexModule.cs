using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoGraphVertexModule : ArangoModule, IArangoGraphVertexModule
    {
        internal ArangoGraphVertexModule(IArangoContext context) : base(context)
        {
        }

        public async Task<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVertexResponse<TR>>(HttpMethod.Get,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}"),
                cancellationToken: cancellationToken);

            return res.Vertex;
        }

        public async Task<ArangoVertexResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<ArangoVoid>>(HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoVertexResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<TR>>(HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoVertexResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<ArangoVoid>>(HttpMethod.Patch,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoVertexResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<TR>>(HttpMethod.Patch,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoVertexResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<ArangoVoid>>(HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoVertexResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph, string collection, T doc, string key,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<TR>>(HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}/{key}"),
                doc, cancellationToken: cancellationToken);
        }

        public async Task<ArangoVertexResponse<ArangoVoid>> RemoveAsync(ArangoHandle database, string graph, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<ArangoVoid>>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}/{key}"), cancellationToken: cancellationToken, throwOnError: false);
        }

        public async Task<ArangoVertexResponse<TR>> RemoveAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoVertexResponse<TR>>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}/{key}"), cancellationToken: cancellationToken, throwOnError: false);
        }
    }
}