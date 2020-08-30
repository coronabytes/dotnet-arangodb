using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoViewModule
    {
        Task CreateAsync(ArangoHandle database, ArangoView view, CancellationToken cancellationToken = default);
        Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default);
        Task DropAsync(ArangoHandle database, string name, CancellationToken cancellationToken = default);
        Task<List<string>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);
    }
}