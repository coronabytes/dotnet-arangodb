using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    /// Implementation of user functions API https://www.arangodb.com/docs/stable/http/aql-user-functions.html
    /// </summary>
    public interface IArangoFunctionModule
    {
        Task<FunctionCreateResponse> CreateAsync(ArangoHandle database, ArangoFunctionDefinition request,
            CancellationToken cancellationToken = default);
        Task<FunctionRemoveResponse> RemoveAsync(ArangoHandle database, FunctionRemoveRequest request,
            CancellationToken cancellationToken = default);
        Task<FunctionListResponse> ListAsync(ArangoHandle database, FunctionListRequest request = default,
            CancellationToken cancellationToken = default);
    }
}