using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoGraphEdgeModule : ArangoModule, IArangoGraphEdgeModule
    {
        internal ArangoGraphEdgeModule(IArangoContext context) : base(context)
        {
        }

        public async ValueTask<TR> GetAsync<TR>(ArangoHandle database, string graph, string collection, string key,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoEdgeResponse<TR>>(database, HttpMethod.Get,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(collection)}"),
                cancellationToken: cancellationToken);

            return res.Edge;
        }

        public async ValueTask<ArangoEdgeResponse<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string graph,
            string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default)
        {
            return await CreateAsync<T, ArangoVoid>(database, graph, collection, doc, waitForSync, returnNew,
                cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string graph,
            string collection, string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await UpdateAsync<T, ArangoVoid>(database, graph, collection, key, doc, waitForSync, keepNull,
                returnNew, returnOld, cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string graph,
            string collection, string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await ReplaceAsync<T, ArangoVoid>(database, graph, collection, key, doc, waitForSync, keepNull,
                returnNew, returnOld, cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<ArangoVoid>> RemoveAsync<T>(ArangoHandle database, string graph,
            string collection, string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            return await RemoveAsync<T, ArangoVoid>(database, graph, collection, key, waitForSync, returnOld,
                cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<TR>> CreateAsync<T, TR>(ArangoHandle database, string graph,
            string collection, T doc,
            bool? waitForSync = null, bool? returnNew = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnNew.HasValue)
                parameter.Add("returnNew", returnNew.Value.ToString().ToLowerInvariant());

            return await SendAsync<ArangoEdgeResponse<TR>>(database, HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}", parameter),
                doc, cancellationToken: cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<TR>> UpdateAsync<T, TR>(ArangoHandle database, string graph,
            string collection, string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (keepNull.HasValue)
                parameter.Add("keepNull", keepNull.Value.ToString().ToLowerInvariant());

            if (returnNew.HasValue)
                parameter.Add("returnNew", returnNew.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            return await SendAsync<ArangoEdgeResponse<TR>>(database, PolyfillHelper.Patch,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}", parameter),
                doc, cancellationToken: cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string graph,
            string collection, string key, T doc,
            bool? waitForSync = null, bool? keepNull = null, bool? returnNew = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (keepNull.HasValue)
                parameter.Add("keepNull", keepNull.Value.ToString().ToLowerInvariant());

            if (returnNew.HasValue)
                parameter.Add("returnNew", returnNew.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            return await SendAsync<ArangoEdgeResponse<TR>>(database, HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}", parameter),
                doc, cancellationToken: cancellationToken);
        }

        public async ValueTask<ArangoEdgeResponse<TR>> RemoveAsync<T, TR>(ArangoHandle database, string graph,
            string collection, string key,
            bool? waitForSync = null, bool? returnOld = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            return await SendAsync<ArangoEdgeResponse<TR>>(database, HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(collection)}/{key}", parameter),
                cancellationToken: cancellationToken);
        }
    }
}