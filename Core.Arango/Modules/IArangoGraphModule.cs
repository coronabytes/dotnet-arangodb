using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoGraphModule
    {
        /// <summary>
        ///   Vertex manipulation module.
        /// </summary>
        IArangoGraphVertexModule Vertex { get; }

        /// <summary>
        ///   Edge manipulation module.
        /// </summary>
        IArangoGraphEdgeModule Edge { get; }

        /// <summary>
        ///  Create a new graph in the graph module.
        /// </summary>
        Task CreateAsync(ArangoHandle database, ArangoGraph request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an existing graph.
        /// </summary>
        Task DropAsync(ArangoHandle database, string name, CancellationToken cancellationToken = default);
        
        /// <summary>
        ///   Lists all graphs known to the graph module.
        /// </summary>
        Task<List<string>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        /// <summary>
        ///   Add an additional vertex collection to the graph.
        /// </summary>
        Task AddVertexCollectionAsync(ArangoHandle database, string graph, ArangoVertexCollection vertexCollection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///   Add a new edge definition to the graph
        /// </summary>
        Task AddEdgeDefinitionAsync(ArangoHandle database, string graph, ArangoEdgeDefinition edgeDefinition,
            CancellationToken cancellationToken = default);
    }
}