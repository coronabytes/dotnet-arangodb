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
            return await SendAsync<T>(null, HttpMethod.Post,
                ApiPath(database, "transaction"),
                request, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ArangoHandle> BeginAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<SingleResult<TransactionResponse>>(null, HttpMethod.Post,
                ApiPath(database, "transaction/begin"),
                request, cancellationToken: cancellationToken).ConfigureAwait(false);

            var transaction = res.Result.Id;
            return new ArangoHandle(database, transaction);
        }

        public async Task CommitAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<ArangoVoid>(null, HttpMethod.Put,
                ApiPath(database, $"transaction/{database.Transaction}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task AbortAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<ArangoVoid>(null, HttpMethod.Delete,
                ApiPath(database, $"transaction/{database.Transaction}"),
                cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}