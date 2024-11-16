using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Database management
    /// </summary>
    public interface IArangoDatabaseModule
    {
        /// <summary>
        ///     Creates a new database
        /// </summary>
        ValueTask<bool> CreateAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Creates a new database with options
        /// </summary>
        ValueTask<bool> CreateAsync(ArangoDatabase database, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Retrieves information about the current database
        /// </summary>
        ValueTask<ArangoDatabaseInfo> GetAsync(ArangoHandle handle, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drop an existing database
        /// </summary>
        ValueTask DropAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Checks if database exists
        /// </summary>
        ValueTask<bool> ExistAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Retrieves a list of all existing databases
        /// </summary>
        ValueTask<List<string>> ListAsync(CancellationToken cancellationToken = default);
    }
}