﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     HotBackup (enterprise only)
    /// </summary>
    public interface IArangoBackupModule
    {
        /// <summary>
        ///     Creates a local backup.
        /// </summary>
        ValueTask<ArangoBackup> CreateAsync(ArangoBackupRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Restores from a local backup.
        /// </summary>
        ValueTask RestoreAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a specific local backup.
        /// </summary>
        ValueTask DeleteAsync(string id, CancellationToken cancellationToken = default);

        /// <summary>
        ///     List all local backups.
        /// </summary>
        ValueTask<List<ArangoBackup>> ListAsync(string id = null, CancellationToken cancellationToken = default);
    }
}