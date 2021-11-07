using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoDatabaseModule : ArangoModule, IArangoDatabaseModule
    {
        internal ArangoDatabaseModule(IArangoContext context) : base(context)
        {
        }

        public async Task<bool> CreateAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVoid>(null, HttpMethod.Post,
                ApiPath("_system", "database"),
                new ArangoDatabase
                {
                    Name = RealmPrefix(name)
                }, false, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<bool> CreateAsync(ArangoDatabase database, CancellationToken cancellationToken = default)
        {
            database.Name = RealmPrefix(database.Name);

            var res = await SendAsync<ArangoVoid>(null, HttpMethod.Post,
                ApiPath("_system", "database"),
                database, false, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<ArangoDatabaseInfo> GetAsync(ArangoHandle handle, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<SingleResult<ArangoDatabaseInfo>>(null, HttpMethod.Get,
                ApiPath(handle, "database/current"), null, false, cancellationToken: cancellationToken);

            return res?.Result;
        }

        public async Task<List<string>> ListAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<string>>(null, HttpMethod.Get,
                ApiPath("_system", "database"), cancellationToken: cancellationToken);

            var realm = string.IsNullOrWhiteSpace(Context.Configuration.Realm)
                ? string.Empty
                : Context.Configuration.Realm + "-";

            return res.Result
                .Where(x => x.StartsWith(realm))
                .Select(x => x.Substring(realm.Length))
                .ToList();
        }

        public async Task<bool> ExistAsync(ArangoHandle handle, CancellationToken cancellationToken = default)
        {
            var db = await GetAsync(handle, cancellationToken);
            return db != null;
        }

        public async Task DropAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(name, HttpMethod.Delete,
                ApiPath("_system", $"database/{RealmPrefix(name)}"), null,
                true, cancellationToken: cancellationToken);
        }
    }
}