using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     AQL user functions
    /// </summary>
    public interface IArangoFunctionModule
    {
        /// <summary>
        ///     create a new AQL user function
        /// </summary>
        /// <returns>true if newly created</returns>
        Task<bool> CreateAsync(ArangoHandle database, ArangoFunctionDefinition request,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     remove an existing AQL user function
        /// </summary>
        /// <param name="database"></param>
        /// <param name="name">the name of the AQL user function</param>
        /// <param name="group">
        ///     The function name provided in name is treated as a namespace prefix, and all functions in the
        ///     specified namespace will be deleted
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns>number of deleted functions</returns>
        Task<int> RemoveAsync(ArangoHandle database, string name, bool? group = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     gets all registered AQL user functions
        /// </summary>
        /// <param name="database"></param>
        /// <param name="ns">filter user functions from namespace</param>
        /// <param name="cancellationToken"></param>
        /// <returns>list of function definitions</returns>
        Task<IReadOnlyCollection<ArangoFunctionDefinition>> ListAsync(ArangoHandle database, string ns = null,
            CancellationToken cancellationToken = default);
    }
}