using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoTransactionModule : ArangoModule, IArangoTransactionModule
    {
        internal ArangoTransactionModule(IArangoContext context) : base(context)
        {
        }

        public async Task<JObject> ExecuteAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, "transaction"),
                Serialize(request), cancellationToken: cancellationToken);
        }

        public async Task<ArangoHandle> BeginAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, "transaction/begin"),
                Serialize(request), cancellationToken: cancellationToken);

            var transaction = res.GetValue("result").Value<string>("id");
            return new ArangoHandle(database, transaction);
        }

        public async Task CommitAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<JObject>(HttpMethod.Put,
                ApiPath(database, $"transaction/{database.Transaction}"),
                cancellationToken: cancellationToken);
        }

        public async Task AbortAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<JObject>(HttpMethod.Delete,
                ApiPath(database, $"transaction/{database.Transaction}"),
                cancellationToken: cancellationToken);
        }
    }
}