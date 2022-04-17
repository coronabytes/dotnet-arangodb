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
                cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<ArangoViewInformation>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<ArangoViewInformation>>(database, HttpMethod.Get,
                ApiPath(database, "view"),
                cancellationToken: cancellationToken);
            return res.Result;
        }

        public async Task<ArangoView> GetPropertiesAsync(ArangoHandle database, string view,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoView>(database, HttpMethod.Get,
                ApiPath(database, $"view/{UrlEncode(view)}/properties"),
                cancellationToken: cancellationToken);
            return res;
        }

        public async Task DropAsync(ArangoHandle database,
            string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"view/{UrlEncode(name)}"),
                cancellationToken: cancellationToken);
        }

        public async Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var views = await ListAsync(database, cancellationToken);

            foreach (var view in views)
                await DropAsync(database, view.Name, cancellationToken);
        }
    }
}