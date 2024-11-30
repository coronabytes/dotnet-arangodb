using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     User management
    /// </summary>
    public interface IArangoUserModule
    {
        /// <summary>
        ///     Create a new user.
        /// </summary>
        ValueTask<bool> CreateAsync(ArangoUser user, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Delete a user permanently.
        /// </summary>
        ValueTask<bool> DeleteAsync(string user, CancellationToken cancellationToken = default);


        /// <summary>
        ///     Clear the database access level, revert back to the default access level
        /// </summary>
        ValueTask<bool> DeleteDatabaseAccessAsync(ArangoHandle handle, string user,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Lists all users
        /// </summary>
        ValueTask<IReadOnlyCollection<ArangoUser>> ListAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///     Modify attributes of an existing user
        /// </summary>
        ValueTask<bool> PatchAsync(ArangoUser user, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Set the database access level.
        /// </summary>
        ValueTask<bool> SetDatabaseAccessAsync(ArangoHandle handle, string user, ArangoAccess access,
            CancellationToken cancellationToken = default);
    }
}