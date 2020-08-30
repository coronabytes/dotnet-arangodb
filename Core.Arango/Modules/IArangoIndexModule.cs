using Core.Arango.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules.Internal
{
    public interface IArangoIndexModule
    {
        Task CreateAsync(ArangoHandle database, string collection, ArangoIndex request, CancellationToken cancellationToken = default);
        Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default);
        Task DropIndexAsync(ArangoHandle database, string index, CancellationToken cancellationToken = default);
        Task<List<string>> ListAsync(ArangoHandle database, string collection, CancellationToken cancellationToken = default);
    }
}