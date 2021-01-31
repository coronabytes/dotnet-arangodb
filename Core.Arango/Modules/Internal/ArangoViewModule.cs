using System.Collections.Generic;
using System.Linq;
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
            await SendAsync<ArangoVoid>(HttpMethod.Post,
                ApiPath(database, "view"),
                view,
                cancellationToken: cancellationToken);
        }

        public async Task<List<string>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<ArangoView>>(HttpMethod.Get,
                ApiPath(database, "view"),
                cancellationToken: cancellationToken);
            return res.Result.Select(x => x.Name).ToList();
        }

        public async Task DropAsync(ArangoHandle database,
            string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath(database, $"view/{UrlEncode(name)}"),
                cancellationToken: cancellationToken);
        }

        public async Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var views = await ListAsync(database, cancellationToken);

            foreach (var view in views)
                await DropAsync(database, view, cancellationToken);
        }
    }
}