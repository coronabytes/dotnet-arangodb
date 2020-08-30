using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    public interface IArangoDatabaseModule
    {
        Task<bool> CreateAsync(ArangoHandle name, CancellationToken cancellationToken = default);
        Task DropAsync(ArangoHandle name, CancellationToken cancellationToken = default);
        Task<bool> ExistAsync(ArangoHandle name, CancellationToken cancellationToken = default);
        Task<List<string>> ListAsync(CancellationToken cancellationToken = default);
    }
}