using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;
using Newtonsoft.Json;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoGraphModule : ArangoModule, IArangoGraphModule
    {
        internal ArangoGraphModule(IArangoContext context) : base(context)
        {
            Vertex = new ArangoGraphVertexModule(context);
            Edge = new ArangoGraphEdgeModule(context);
        }

        public async ValueTask<IReadOnlyCollection<ArangoGraph>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<GraphsResponse<ArangoGraph>>(database, HttpMethod.Get,
                ApiPath(database, "gharial"), cancellationToken: cancellationToken);
            return res.Graphs;
        }

        public async ValueTask<ArangoGraph> GetAsync(ArangoHandle database, string graph,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<GraphResponse<ArangoGraph>>(database, HttpMethod.Get,
                    ApiPath(database, $"gharial/{UrlEncode(graph)}"), cancellationToken: cancellationToken)
                .ConfigureAwait(false);
            return res.Graph;
        }

        public IArangoGraphVertexModule Vertex { get; }
        public IArangoGraphEdgeModule Edge { get; }

        public async ValueTask CreateAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, "gharial"),
                request, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask AddVertexCollectionAsync(ArangoHandle database, string graph,
            ArangoVertexCollection vertexCollection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex"),
                vertexCollection, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask RemoveVertexCollectionAsync(ArangoHandle database, string graph, string vertexCollection,
            bool? dropCollection = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (dropCollection.HasValue)
                parameter.Add("dropCollection", dropCollection.Value.ToString().ToLowerInvariant());

            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(vertexCollection)}", parameter),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask AddEdgeDefinitionAsync(ArangoHandle database, string graph,
            ArangoEdgeDefinition edgeDefinition,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge"),
                edgeDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask ReplaceEdgeDefinitionAsync(ArangoHandle database, string graph,
            ArangoEdgeDefinition edgeDefinition,
            bool? dropCollections = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (dropCollections.HasValue)
                parameter.Add("dropCollections", dropCollections.Value.ToString().ToLowerInvariant());

            await SendAsync<ArangoVoid>(database, HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(edgeDefinition.Collection)}", parameter),
                edgeDefinition, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask RemoveEdgeDefinitionAsync(ArangoHandle database, string graph, string edgeDefinition,
            bool? dropCollections = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (dropCollections.HasValue)
                parameter.Add("dropCollections", dropCollections.Value.ToString().ToLowerInvariant());

            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(edgeDefinition)}", parameter),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask DropAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(name)}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        private class GraphRes
        {
            [JsonPropertyName("_key")]
            [JsonProperty(PropertyName = "_key")]
            public string Key { get; set; }
        }
    }
}