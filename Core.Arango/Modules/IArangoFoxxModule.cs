using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    public interface IArangoFoxxModule
    {
        /// <summary>
        ///   List installed services
        /// </summary>
        /// <param name="database"></param>
        /// <param name="excludeSystem">Whether or not system services should be excluded from the result.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ICollection<ArangoFoxxService>> ListServicesAsync(ArangoHandle database, bool? excludeSystem = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Install new service
        /// </summary>
        /// <param name="database">Database the service should be installed at.</param>
        /// <param name="mount">Mount path the service should be installed at.</param>
        /// <param name="service"></param>
        /// <param name="development">Set to true to enable development mode.</param>
        /// <param name="setup">Set to false to not run the service’s setup script.</param>
        /// <param name="legacy">Set to true to install the service in 2.8 legacy compatibility mode.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArangoVoid> InstallServiceAsync(ArangoHandle database, string mount, ArangoFoxxSource service,
            bool? development = null, bool? setup = null, bool? legacy = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace a service
        /// </summary>
        /// <param name="database">Database the service should be installed at.</param>
        /// <param name="mount">Mount path the service should be installed at.</param>
        /// <param name="service"></param>
        /// <param name="teardown">Set to true to run the old service’s teardown script.</param>
        /// <param name="setup">Set to false to not run the service’s setup script.</param>
        /// <param name="legacy">Set to true to install the service in 2.8 legacy compatibility mode.</param>
        /// <param name="force">Set to true to force service install even if no service is installed under given mount.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArangoVoid> ReplaceServiceAsync(ArangoHandle database, string mount, ArangoFoxxSource service,
            bool? teardown = null, bool? setup = null, bool? legacy = null, bool? force = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Upgrade a service
        /// </summary>
        /// <param name="database">Database the service should be installed at.</param>
        /// <param name="mount">Mount path the service should be installed at.</param>
        /// <param name="service"></param>
        /// <param name="teardown">Set to true to run the old service’s teardown script.</param>
        /// <param name="setup">Set to false to not run the service’s setup script.</param>
        /// <param name="legacy">Set to true to install the service in 2.8 legacy compatibility mode.</param>
        /// <param name="force">Set to true to force service install even if no service is installed under given mount.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ArangoVoid> UpgradeServiceAsync(ArangoHandle database, string mount, ArangoFoxxSource service,
            bool? teardown = null, bool? setup = null, bool? legacy = null, bool? force = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Uninstall service
        /// </summary>
        Task<ArangoVoid> UninstallServiceAsync(ArangoHandle database, string mount,
            bool? teardown = null,
            CancellationToken cancellationToken = default);

        Task<T> GetConfigurationAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        Task<ArangoVoid> UpdateConfigurationAsync(ArangoHandle database, string mount,
            object configuration = null,
            CancellationToken cancellationToken = default);

        Task<ArangoVoid> ReplaceConfigurationAsync(ArangoHandle database, string mount,
            object configuration = null,
            CancellationToken cancellationToken = default);

        Task<T> GetDependenciesAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        Task<ArangoVoid> UpdateDependenciesAsync(ArangoHandle database, string mount,
            object dependencies = null,
            CancellationToken cancellationToken = default);

        Task<ArangoVoid> ReplaceDependenciesAsync(ArangoHandle database, string mount,
            object dependencies = null,
            CancellationToken cancellationToken = default);
    }
}