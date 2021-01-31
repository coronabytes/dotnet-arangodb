using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoTransactionModule : ArangoModule, IArangoTransactionModule
    {
        internal ArangoTransactionModule(IArangoContext context) : base(context)
        {
        }

        public async Task<T> ExecuteAsync<T>(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<T>(HttpMethod.Post,
                ApiPath(database, "transaction"),
                request, cancellationToken: cancellationToken);
        }

        public async Task<ArangoHandle> BeginAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<SingleResult<TransactionResponse>>(HttpMethod.Post,
                ApiPath(database, "transaction/begin"),
                request, cancellationToken: cancellationToken);

            var transaction = res.Result.Id;
            return new ArangoHandle(database, transaction);
        }

        public async Task CommitAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<ArangoVoid>(HttpMethod.Put,
                ApiPath(database, $"transaction/{database.Transaction}"),
                cancellationToken: cancellationToken);
        }

        public async Task AbortAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<ArangoVoid>(HttpMethod.Delete,
                ApiPath(database, $"transaction/{database.Transaction}"),
                cancellationToken: cancellationToken);
        }
    }
}