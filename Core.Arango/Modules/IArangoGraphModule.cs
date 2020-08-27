using Core.Arango.Protocol;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    public interface IArangoGraphModule
    {
        Task CreateAsync(ArangoHandle database, ArangoGraph request, CancellationToken cancellationToken = default);
        Task DropAsync(ArangoHandle database, string name, CancellationToken cancellationToken = default);
        Task<List<string>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);
    }
}