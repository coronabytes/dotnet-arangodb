using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        Task<bool> CreateAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drop an existing database
        /// </summary>
        Task DropAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Checks if database exists (calls ListAsync internally)
        /// </summary>
        Task<bool> ExistAsync(ArangoHandle name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Retrieves a list of all existing databases
        /// </summary>
        Task<List<string>> ListAsync(CancellationToken cancellationToken = default);
    }
}