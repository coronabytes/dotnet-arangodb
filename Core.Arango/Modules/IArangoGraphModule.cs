using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Graph management
    /// </summary>
    public interface IArangoGraphModule
    {
        /// <summary>
        ///     Vertex manipulation module.
        /// </summary>
        IArangoGraphVertexModule Vertex { get; }

        /// <summary>
        ///     Edge manipulation module.
        /// </summary>
        IArangoGraphEdgeModule Edge { get; }

        /// <summary>
        ///     Create a new graph in the graph module.
        /// </summary>
        Task CreateAsync(ArangoHandle database, ArangoGraph request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete an existing graph.
        /// </summary>
        Task DropAsync(ArangoHandle database, string name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Lists all graphs known to the graph module.
        /// </summary>
        ValueTask<IReadOnlyCollection<ArangoGraph>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get a graph from the graph module.
        /// </summary>
        ValueTask<ArangoGraph> GetAsync(ArangoHandle database, string graph, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Add an additional vertex collection to the graph.
        /// </summary>
        Task AddVertexCollectionAsync(ArangoHandle database, string graph, ArangoVertexCollection vertexCollection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Add a new edge definition to the graph
        /// </summary>
        Task AddEdgeDefinitionAsync(ArangoHandle database, string graph, ArangoEdgeDefinition edgeDefinition,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Remove a vertex collection form the graph.
        /// </summary>
        Task RemoveVertexCollectionAsync(ArangoHandle database, string graph, string vertexCollection,
            bool? dropCollection = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace an existing edge definition
        /// </summary>
        /// <returns></returns>
        Task ReplaceEdgeDefinitionAsync(ArangoHandle database, string graph, ArangoEdgeDefinition edgeDefinition,
            bool? dropCollections = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Remove an edge definition form the graph
        /// </summary>
        Task RemoveEdgeDefinitionAsync(ArangoHandle database, string graph, string edgeDefinition,
            bool? dropCollections = null,
            CancellationToken cancellationToken = default);
    }
}