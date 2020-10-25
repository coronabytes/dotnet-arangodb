using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoIndexModule : ArangoModule, IArangoIndexModule
    {
        internal ArangoIndexModule(IArangoContext context) : base(context)
        {
        }

        public async Task CreateAsync(ArangoHandle database, string collection, ArangoIndex request,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, $"index?collection={collection}"),
                request, cancellationToken: cancellationToken);
        }

        /// <summary>
        ///     Drops all user created indices over all collections in database
        /// </summary>
        public async Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var collections = await Context.Collection.ListAsync(database, cancellationToken);

            foreach (var col in collections)
            {
                var indices = await ListAsync(database, col, cancellationToken);

                foreach (var idx in indices)
                    await DropAsync(database, idx, cancellationToken);
            }
        }

        /// <summary>
        ///     Ignores primary and edge indices
        /// </summary>
        public async Task<List<string>> ListAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Get,
                ApiPath(database, $"index?collection={UrlEncode(collection)}"),
                cancellationToken: cancellationToken);
            return res.GetValue("indexes")
                .Where(x =>
                {
                    var type = x.Value<string>("type");
                    return type != "primary" && type != "edge";
                })
                .Select(x => x.Value<string>("id")).ToList();
        }

        public async Task DropAsync(ArangoHandle database, string index,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                ApiPath(database, $"index/{index}"), cancellationToken: cancellationToken);
        }
    }
}