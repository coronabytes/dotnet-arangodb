using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json.Linq;

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
            await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, "view"),
                Serialize(view),
                cancellationToken: cancellationToken);
        }

        public async Task<List<string>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get,
                ApiPath(database, "view"),
                cancellationToken: cancellationToken);
            return res.GetValue("result")
                .Select(x => x.Value<string>("name")).ToList();
        }

        public async Task DropAsync(ArangoHandle database,
            string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
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