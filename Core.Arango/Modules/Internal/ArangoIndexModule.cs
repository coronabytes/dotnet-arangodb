using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;

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
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, $"index?collection={collection}"),
                request, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///     Drops all user created indices over all collections in database
        /// </summary>
        public async Task DropAllAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            var collections = await Context.Collection.ListAsync(database, cancellationToken).ConfigureAwait(false);

            foreach (var col in collections)
            {
                var indices = await ListAsync(database, col.Name, cancellationToken).ConfigureAwait(false);

                foreach (var idx in indices)
                    await DropAsync(database, idx.Id, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Ignores primary and edge indices
        /// </summary>
        public async Task<IReadOnlyCollection<ArangoIndex>> ListAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<IndexResponse>(database, HttpMethod.Get,
                ApiPath(database, $"index?collection={UrlEncode(collection)}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);

            return res.Indexes
                .Where(x =>
                {
                    var type = x.Type;
                    return type != ArangoIndexType.Primary && type != ArangoIndexType.Edge;
                }).ToList();
        }

        public async Task DropAsync(ArangoHandle database, string index,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"index/{index}"), cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        private class IndexResponse
        {
            [JsonProperty("indexes")]
            [JsonPropertyName("indexes")]
            public List<ArangoIndex> Indexes { get; set; }
        }
    }
}