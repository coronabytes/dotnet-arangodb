using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

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
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, "collection"),
                new ArangoCollection
                {
                    Name = collection,
                    Type = type
                }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task CreateAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Post,
                ApiPath(database, "collection"),
                collection,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task TruncateAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Put,
                ApiPath(database, $"collection/{UrlEncode(collection)}/truncate"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<ArangoCollection>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<ArangoCollection>>(database, HttpMethod.Get,
                ApiPath(database, "collection?excludeSystem=true"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
            return res.Result;
        }

        public async Task UpdateAsync(ArangoHandle database, string collection, ArangoCollectionUpdate update,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Put,
                ApiPath(database, $"collection/{UrlEncode(collection)}/properties"),
                update,
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task RenameAsync(ArangoHandle database, string oldname, string newname,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Put,
                ApiPath(database, $"collection/{UrlEncode(oldname)}/rename"),
                new
                {
                    name = newname
                },
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<bool> ExistAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            var collections = await ListAsync(database, cancellationToken).ConfigureAwait(false);

            return collections.Any(x => x.Name.Equals(collection));
        }

        public async Task CompactAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(
                database, HttpMethod.Put,
                ApiPath(database, $"collection/{UrlEncode(collection)}/compact"),
                null, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task RecalculateCountAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(
                database, HttpMethod.Put,
                ApiPath(database, $"collection/{UrlEncode(collection)}/recalculateCount"),
                null, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ArangoCollection> GetAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoCollection>(
                database, HttpMethod.Get,
                ApiPath(database, $"collection/{UrlEncode(collection)}"),
                null, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task DropAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                ApiPath(database, $"collection/{UrlEncode(collection)}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}