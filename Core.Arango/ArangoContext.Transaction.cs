using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        public async Task<ArangoHandle> BeginTransactionAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{Server}/_db/{DbName(database)}/_api/transaction/begin",
                JsonConvert.SerializeObject(request, JsonSerializerSettings), cancellationToken: cancellationToken);

            var transaction = res.GetValue("result").Value<string>("id");
            return new ArangoHandle(database, transaction);
        }

        public async Task CommitTransactionAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<JObject>(HttpMethod.Put,
                $"{Server}/_db/{DbName(database)}/_api/transaction/{database.Transaction}",
                cancellationToken: cancellationToken);
        }

        public async Task AbortTransactionAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<JObject>(HttpMethod.Delete,
                $"{Server}/_db/{DbName(database)}/_api/transaction/{database.Transaction}",
                cancellationToken: cancellationToken);
        }
    }
}