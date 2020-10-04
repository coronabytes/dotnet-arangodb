using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoCollectionModule : ArangoModule, IArangoCollectionModule
    {
        public ArangoCollectionModule(IArangoContext context) : base(context)
        {
        }

        public async Task CreateAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, "collection"),
                Serialize(new ArangoCollection
                {
                    Name = collection,
                    Type = type
                }), cancellationToken: cancellationToken);
        }

        public async Task CreateAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, "collection"),
                JsonConvert.SerializeObject(collection, ArangoContext.JsonSerializerSettings),
                cancellationToken: cancellationToken);
        }

        public async Task TruncateAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Put,
                ApiPath(database, $"collection/{UrlEncode(collection)}/truncate"),
                cancellationToken: cancellationToken);
        }

        public async Task<List<string>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<JObject>>(HttpMethod.Get,
                ApiPath(database, "collection?excludeSystem=true"),
                cancellationToken: cancellationToken);
            return res.Result.Select(x => x.Value<string>("name")).ToList();
        }

        public async Task UpdateAsync(ArangoHandle database, string collection, ArangoCollectionUpdate update,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Put,
                ApiPath(database, $"collection/{collection}/properties"),
                Serialize(update),
                cancellationToken: cancellationToken);
        }

        public async Task RenameAsync(ArangoHandle database, string oldname, string newname,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Put,
                ApiPath(database, $"collection/{oldname}/rename"),
                Serialize(new
                {
                    name = newname
                }),
                cancellationToken: cancellationToken);
        }

        public async Task DropCollectionAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                ApiPath(database, $"collection/){UrlEncode(collection)}"),
                cancellationToken: cancellationToken);
        }
    }
}