using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoBackupModule
    {
        /// <summary>
        /// Creates a local backup.
        /// </summary>
        Task<ArangoBackup> CreateAsync(ArangoBackupRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Restores from a local backup.
        /// </summary>
        Task RestoreAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a specific local backup.
        /// </summary>
        Task DeleteAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        ///  List all local backups.
        /// </summary>
        Task<List<ArangoBackup>> ListAsync(string id = null, CancellationToken cancellationToken = default);
    }
}