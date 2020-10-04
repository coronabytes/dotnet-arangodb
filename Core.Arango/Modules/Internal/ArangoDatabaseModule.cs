using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoDatabaseModule : ArangoModule, IArangoDatabaseModule
    {
        internal ArangoDatabaseModule(IArangoContext context) : base(context)
        {
        }

        public async Task<bool> CreateAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                ApiPath("_system", "database"),
                Serialize(new
                {
                    name = RealmPrefix(name)
                }), throwOnError: false, cancellationToken: cancellationToken);

            return res != null;
        }

        public async Task<List<string>> ListAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<string>>(HttpMethod.Get,
                ApiPath("_system", "database"), cancellationToken: cancellationToken);

            return res.Result
                .Where(x => x.StartsWith(Realm))
                .Select(x => x.Substring(Realm.Length))
                .ToList();
        }

        public async Task<bool> ExistAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            var dbs = await ListAsync(cancellationToken);

            return dbs.Contains(name);
        }

        public async Task DropAsync(ArangoHandle name, CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                ApiPath("_system", $"database/{RealmPrefix(name)}"), null,
                throwOnError: false, cancellationToken: cancellationToken);
        }
    }
}