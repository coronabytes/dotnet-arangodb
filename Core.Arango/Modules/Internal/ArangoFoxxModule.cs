using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoFoxxModule : ArangoModule, IArangoFoxxModule
    {
        internal ArangoFoxxModule(IArangoContext context) : base(context)
        {
        }

        public async Task<ArangoVoid> ListServicesAsync(ArangoHandle database, bool? excludeSystem = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (excludeSystem.HasValue)
                parameter.Add("excludeSystem", excludeSystem.Value.ToString().ToLowerInvariant());

            return await SendAsync<ArangoVoid>(HttpMethod.Get,
                ApiPath(database, "foxx", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task<ArangoVoid> InstallServiceAsync(ArangoHandle database,
            string mount, MultipartFormDataContent content,
            bool? development = null, bool? setup = null, bool? legacy = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            if (development.HasValue)
                parameter.Add("development", development.Value.ToString().ToLowerInvariant());

            if (setup.HasValue)
                parameter.Add("setup", setup.Value.ToString().ToLowerInvariant());

            if (legacy.HasValue)
                parameter.Add("legacy", legacy.Value.ToString().ToLowerInvariant());

            var res = await Context.Configuration.Transport.SendContentAsync<HttpContent>(HttpMethod.Post,
                ApiPath(database, "foxx", parameter), content,
                cancellationToken: cancellationToken);

            return new ArangoVoid();
        }

        public async Task<ArangoVoid> ReplaceServiceAsync(ArangoHandle database,
            string mount, MultipartFormDataContent content,
            bool? teardown = null, bool? setup = null, bool? legacy = null, bool? force = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            if (teardown.HasValue)
                parameter.Add("teardown", teardown.Value.ToString().ToLowerInvariant());

            if (setup.HasValue)
                parameter.Add("setup", setup.Value.ToString().ToLowerInvariant());

            if (legacy.HasValue)
                parameter.Add("legacy", legacy.Value.ToString().ToLowerInvariant());

            if (force.HasValue)
                parameter.Add("force", force.Value.ToString().ToLowerInvariant());

            var res = await Context.Configuration.Transport.SendContentAsync<HttpContent>(HttpMethod.Put,
                ApiPath(database, "foxx", parameter), content,
                cancellationToken: cancellationToken);

            return new ArangoVoid();
        }

        public async Task<ArangoVoid> UpgradeServiceAsync(ArangoHandle database,
            string mount, MultipartFormDataContent content,
            bool? teardown = null, bool? setup = null, bool? legacy = null, bool? force = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            if (teardown.HasValue)
                parameter.Add("teardown", teardown.Value.ToString().ToLowerInvariant());

            if (setup.HasValue)
                parameter.Add("setup", setup.Value.ToString().ToLowerInvariant());

            if (legacy.HasValue)
                parameter.Add("legacy", legacy.Value.ToString().ToLowerInvariant());

            if (force.HasValue)
                parameter.Add("force", force.Value.ToString().ToLowerInvariant());

            var res = await Context.Configuration.Transport.SendContentAsync<HttpContent>(HttpMethod.Patch,
                ApiPath(database, "foxx", parameter), content,
                cancellationToken: cancellationToken);

            return new ArangoVoid();
        }

        public async Task<ArangoVoid> UninstallServiceAsync(ArangoHandle database, string mount,
            bool? teardown = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            if (teardown.HasValue)
                parameter.Add("teardown", teardown.Value.ToString().ToLowerInvariant());

            return await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath(database, "foxx/service", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task<T> GetConfigurationAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            return await SendAsync<T>(HttpMethod.Get,
                ApiPath(database, "foxx/configuration", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task<ArangoVoid> UpdateConfigurationAsync(ArangoHandle database, string mount,
            object configuration,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            return await SendAsync<ArangoVoid>(HttpMethod.Patch,
                ApiPath(database, "foxx/configuration", parameter), configuration,
                cancellationToken: cancellationToken);
        }

        public async Task<ArangoVoid> ReplaceConfigurationAsync(ArangoHandle database, string mount,
            object configuration,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            return await SendAsync<ArangoVoid>(HttpMethod.Put,
                ApiPath(database, "foxx/configuration", parameter), configuration,
                cancellationToken: cancellationToken);
        }

        public async Task<T> GetDependenciesAsync<T>(ArangoHandle database, string mount,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            return await SendAsync<T>(HttpMethod.Get,
                ApiPath(database, "foxx/dependencies", parameter),
                cancellationToken: cancellationToken);
        }

        public async Task<ArangoVoid> UpdateDependenciesAsync(ArangoHandle database, string mount,
            object dependencies,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            return await SendAsync<ArangoVoid>(HttpMethod.Patch,
                ApiPath(database, "foxx/dependencies", parameter), dependencies,
                cancellationToken: cancellationToken);
        }

        public async Task<ArangoVoid> ReplaceDependenciesAsync(ArangoHandle database, string mount,
            object dependencies,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"mount", mount}};

            return await SendAsync<ArangoVoid>(HttpMethod.Put,
                ApiPath(database, "foxx/dependencies", parameter), dependencies,
                cancellationToken: cancellationToken);
        }
    }
}