using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Index management
    /// </summary>
    public interface IArangoIndexModule
    {
        /// <summary>
        ///     creates an index
        /// </summary>
        Task CreateAsync(ArangoHandle database, string collection, ArangoIndex request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops all indices of a database
        /// </summary>
        Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops an index
        /// </summary>
        Task DropAsync(ArangoHandle database, string index, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns all indexes of a collection
        /// </summary>
        Task<IReadOnlyCollection<ArangoIndex>> ListAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);
    }
}