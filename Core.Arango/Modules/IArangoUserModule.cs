using Core.Arango.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    public interface IArangoUserModule
    {
        Task<bool> CreateAsync(ArangoUser user, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string user, CancellationToken cancellationToken = default);
        Task<bool> DeleteDatabaseAccessAsync(ArangoHandle handle, string user, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<ArangoUser>> ListAsync(CancellationToken cancellationToken = default);
        Task<bool> PatchAsync(ArangoUser user, CancellationToken cancellationToken = default);
        Task<bool> SetDatabaseAccessAsync(ArangoHandle handle, string user, ArangoAccess access, CancellationToken cancellationToken = default);
    }
}