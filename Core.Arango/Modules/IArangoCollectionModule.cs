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
        ValueTask CreateAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates a collection
        /// </summary>
        ValueTask CreateAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns all collections
        /// </summary>
        ValueTask<IReadOnlyCollection<ArangoCollection>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Renames a collection (not in cluster)
        /// </summary>
        ValueTask RenameAsync(ArangoHandle database, string oldname, string newname,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Truncates a collection
        /// </summary>
        ValueTask TruncateAsync(ArangoHandle database, string collection, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Changes a collection
        /// </summary>
        ValueTask UpdateAsync(ArangoHandle database, string collection, ArangoCollectionUpdate update,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops a collection
        /// </summary>
        /// <returns></returns>
        ValueTask DropAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns a collection
        /// </summary>
        ValueTask<ArangoCollection> GetAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Checks of collection exists (calls ListAsync internally)
        /// </summary>
        ValueTask<bool> ExistAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Compacts the data of a collection in order to reclaim disk space
        /// </summary>
        ValueTask CompactAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Recalculates the document count of a collection, if it ever becomes inconsistent.
        /// </summary>
        ValueTask RecalculateCountAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default);
    }
}