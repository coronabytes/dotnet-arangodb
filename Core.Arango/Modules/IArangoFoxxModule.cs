using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Foxx service management and execution
    /// </summary>
    public interface IArangoFoxxModule
    {
        /// <summary>
        ///     List installed services
        /// </summary>
        /// <param name="database"></param>
        /// <param name="excludeSystem">Whether or not system services should be excluded from the result.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<ICollection<ArangoFoxxService>> ListServicesAsync(ArangoHandle database, bool? excludeSystem = null,
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
        ValueTask<ArangoVoid> InstallServiceAsync(ArangoHandle database, string mount, ArangoFoxxSource service,
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
        ValueTask<ArangoVoid> ReplaceServiceAsync(ArangoHandle database, string mount, ArangoFoxxSource service,
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
        ValueTask<ArangoVoid> UpgradeServiceAsync(ArangoHandle database, string mount, ArangoFoxxSource service,
            bool? teardown = null, bool? setup = null, bool? legacy = null, bool? force = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Uninstall service
        /// </summary>
        ValueTask<ArangoVoid> UninstallServiceAsync(ArangoHandle database, string mount,
            bool? teardown = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get configuration options
        /// </summary>
        ValueTask<T> GetConfigurationAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update configuration options
        /// </summary>
        ValueTask<ArangoVoid> UpdateConfigurationAsync(ArangoHandle database, string mount,
            object configuration = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace configuration options
        /// </summary>
        ValueTask<ArangoVoid> ReplaceConfigurationAsync(ArangoHandle database, string mount,
            object configuration = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Get dependency options
        /// </summary>
        ValueTask<T> GetDependenciesAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Update dependency options
        /// </summary>
        ValueTask<ArangoVoid> UpdateDependenciesAsync(ArangoHandle database, string mount,
            object dependencies = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Replace dependency options
        /// </summary>
        ValueTask<ArangoVoid> ReplaceDependenciesAsync(ArangoHandle database, string mount,
            object dependencies = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     HTTP GET request to Foxx service
        /// </summary>
        ValueTask<T> GetAsync<T>(ArangoHandle database, string path,
            IDictionary<string, string> queryParams = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     HTTP POST request to Foxx service
        /// </summary>
        ValueTask<T> PostAsync<T>(ArangoHandle database, string path, object body,
            IDictionary<string, string> queryParams = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     HTTP PUT request to Foxx service
        /// </summary>
        ValueTask<T> PutAsync<T>(ArangoHandle database, string path, object body,
            IDictionary<string, string> queryParams = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     HTTP PATCH request to Foxx service
        /// </summary>
        ValueTask<T> PatchAsync<T>(ArangoHandle database, string path, object body,
            IDictionary<string, string> queryParams = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     HTTP DELETE request to Foxx service
        /// </summary>
        ValueTask<T> DeleteAsync<T>(ArangoHandle database, string path,
            IDictionary<string, string> queryParams = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Enable development mode
        /// </summary>
        /// <param name="database">Database the target service.</param>
        /// <param name="mount">Mount path of the installed service.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask EnableDevelopmentModeAsync(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Disable development mode
        /// </summary>
        /// <param name="database">Database the target service.</param>
        /// <param name="mount">Mount path of the installed service.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask DisableDevelopmentModeAsync(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="mount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<Stream> DownloadServiceAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="mount"></param>
        /// <param name="name"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask<T> RunServiceScriptAsync<T>(ArangoHandle database, string mount, string name,
            object body = null,
            CancellationToken cancellationToken = default);
    }
}