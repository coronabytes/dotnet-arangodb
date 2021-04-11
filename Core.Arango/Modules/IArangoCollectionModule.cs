using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Collection management
    /// </summary>
    public interface IArangoCollectionModule
    {
        /// <summary>
        ///     Creates a collection
        /// </summary>
        Task CreateAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates a collection
        /// </summary>
        Task CreateAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns all collections
        /// </summary>
        Task<IReadOnlyCollection<ArangoCollection>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Renames a collection (not in cluster)
        /// </summary>
        Task RenameAsync(ArangoHandle database, string oldname, string newname,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Truncates a collection
        /// </summary>
        Task TruncateAsync(ArangoHandle database, string collection, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Changes a collection
        /// </summary>
        Task UpdateAsync(ArangoHandle database, string collection, ArangoCollectionUpdate update,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops a collection
        /// </summary>
        /// <returns></returns>
        Task DropAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns a collection
        /// </summary>
        Task<ArangoCollection> GetAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Checks of collection exists (calls ListAsync internally)
        /// </summary>
        Task<bool> ExistAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);
    }
}