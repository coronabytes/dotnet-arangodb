using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Arango View (ArangoSearch) management
    /// </summary>
    public interface IArangoViewModule
    {
        /// <summary>
        ///     creates an ArangoSearch View
        /// </summary>
        Task CreateAsync(ArangoHandle database, ArangoView view, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops all Views in database
        /// </summary>
        Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Drops a View
        /// </summary>
        Task DropAsync(ArangoHandle database, string name, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Returns all Views
        /// </summary>
        Task<IReadOnlyCollection<ArangoViewInformation>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Get properties of view
        /// </summary>
        /// <param name="database"></param>
        /// <param name="view"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArangoView> GetPropertiesAsync(ArangoHandle database, string view, CancellationToken cancellationToken = default);
    }
}