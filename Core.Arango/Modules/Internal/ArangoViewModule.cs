using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoViewModule : ArangoModule, IArangoViewModule
    {
        internal ArangoViewModule(IArangoContext context) : base(context)
        {
        }

        public async Task CreateAsync(ArangoHandle database, ArangoView view,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, "view"),
                view,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(ArangoHandle database, ArangoViewUpdate view, CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Put,
                ApiPath(database, $"view/{UrlEncode(view.Name)}/properties"),
                view,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task PatchAsync(ArangoHandle database, ArangoViewPatch view, CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, PolyfillHelper.Patch,
                ApiPath(database, $"view/{UrlEncode(view.Name)}/properties"),
                view,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<IReadOnlyCollection<ArangoViewInformation>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<ArangoViewInformation>>(database, HttpMethod.Get,
                ApiPath(database, "view"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
            return res.Result;
        }

        public async ValueTask<ArangoView> GetPropertiesAsync(ArangoHandle database, string view,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoView>(database, HttpMethod.Get,
                ApiPath(database, $"view/{UrlEncode(view)}/properties"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task DropAsync(ArangoHandle database,
            string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"view/{UrlEncode(name)}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var views = await ListAsync(database, cancellationToken).ConfigureAwait(false);

            foreach (var view in views)
                await DropAsync(database, view.Name, cancellationToken).ConfigureAwait(false);
        }
    }
}