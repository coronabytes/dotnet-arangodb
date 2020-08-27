using Core.Arango.Protocol;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    public interface IArangoTransactionModule
    {
        Task AbortAsync(ArangoHandle database, CancellationToken cancellationToken = default);
        Task<ArangoHandle> BeginAsync(ArangoHandle database, ArangoTransaction request, CancellationToken cancellationToken = default);
        Task CommitAsync(ArangoHandle database, CancellationToken cancellationToken = default);
        Task<JObject> ExecuteAsync(ArangoHandle database, ArangoTransaction request, CancellationToken cancellationToken = default);
    }
}