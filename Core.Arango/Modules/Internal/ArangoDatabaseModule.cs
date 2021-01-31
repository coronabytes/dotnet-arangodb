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
            var res = await SendAsync<ArangoVoid>(HttpMethod.Post,
                ApiPath("_system", "database"),
                new
                {
                    name = RealmPrefix(name)
                }, throwOnError: false, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<List<string>> ListAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<string>>(HttpMethod.Get,
                ApiPath("_system", "database"), cancellationToken: cancellationToken);

            var realm = string.IsNullOrWhiteSpace(Context.Configuration.Realm)
                ? string.Empty
                : Context.Configuration.Realm + "-";

            return res.Result
                .Where(x => x.StartsWith(realm))
                .Select(x => x.Substring(realm.Length))
                .ToList();
        }

        public async Task<bool> ExistAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            var dbs = await ListAsync(cancellationToken);

            return dbs.Contains(name);
        }

        public async Task DropAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath("_system", $"database/{RealmPrefix(name)}"), null,
                throwOnError: false, cancellationToken: cancellationToken);
        }
    }
}