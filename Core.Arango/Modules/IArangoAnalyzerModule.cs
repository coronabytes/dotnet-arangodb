using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoAnalyzerModule
    {
        Task CreateAsync(ArangoHandle database, ArangoAnalyzer analyzer, CancellationToken cancellationToken = default);

        Task DeleteAsync(ArangoHandle database, string analyzer, bool force = false,
            CancellationToken cancellationToken = default);

        Task<List<ArangoAnalyzer>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);
    }
}