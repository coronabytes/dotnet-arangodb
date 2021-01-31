using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<string>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<GraphResponse<GraphRes>>(HttpMethod.Get,
                ApiPath(database, "gharial"), cancellationToken: cancellationToken);
            return res.Graphs.Select(x => x.Key).ToList();
        }

        public IArangoGraphVertexModule Vertex { get; }
        public IArangoGraphEdgeModule Edge { get; }

        public async Task CreateAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(HttpMethod.Post,
                ApiPath(database, "gharial"),
                request, cancellationToken: cancellationToken);
        }

        public async Task AddVertexCollectionAsync(ArangoHandle database, string graph,
            ArangoVertexCollection vertexCollection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex"),
                vertexCollection, cancellationToken: cancellationToken);
        }

        public async Task RemoveVertexCollectionAsync(ArangoHandle database, string graph, string vertexCollection,
            bool? dropCollection = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (dropCollection.HasValue)
                parameter.Add("dropCollection", dropCollection.Value.ToString().ToLowerInvariant());

            await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/vertex/{UrlEncode(vertexCollection)}", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task AddEdgeDefinitionAsync(ArangoHandle database, string graph,
            ArangoEdgeDefinition edgeDefinition,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(HttpMethod.Post,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge"),
                edgeDefinition, cancellationToken: cancellationToken);
        }

        public async Task ReplaceEdgeDefinitionAsync(ArangoHandle database, string graph,
            ArangoEdgeDefinition edgeDefinition,
            bool? dropCollections = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (dropCollections.HasValue)
                parameter.Add("dropCollections", dropCollections.Value.ToString().ToLowerInvariant());

            await SendAsync<ArangoVoid>(HttpMethod.Put,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(edgeDefinition.Collection)}", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task RemoveEdgeDefinitionAsync(ArangoHandle database, string graph, string edgeDefinition,
            bool? dropCollections = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (dropCollections.HasValue)
                parameter.Add("dropCollections", dropCollections.Value.ToString().ToLowerInvariant());

            await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(graph)}/edge/{UrlEncode(edgeDefinition)}", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task DropAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(name)}"),
                cancellationToken: cancellationToken);
        }

        private class GraphRes
        {
            [JsonPropertyName("_key")]
            [JsonProperty(PropertyName = "_key")]
            public string Key { get; set; }
        }
    }
}