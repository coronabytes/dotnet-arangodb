using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoAnalyzerModule : ArangoModule, IArangoAnalyzerModule
    {
        internal ArangoAnalyzerModule(IArangoContext context) : base(context)
        {
        }

        public async ValueTask<IReadOnlyCollection<ArangoAnalyzer>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            return (await SendAsync<QueryResponse<ArangoAnalyzer>>(database, HttpMethod.Get,
                ApiPath(database, "analyzer"),
                cancellationToken: cancellationToken)).Result;
        }

        public async ValueTask<ArangoAnalyzer> GetDefinitionAsync(ArangoHandle database, string analyzer,
            CancellationToken cancellationToken = default)
        {
            return await SendAsync<ArangoAnalyzer>(database, HttpMethod.Get,
                ApiPath(database, $"analyzer/{UrlEncode(analyzer)}"),
                cancellationToken: cancellationToken);
        }

        public async ValueTask CreateAsync(ArangoHandle database,
            ArangoAnalyzer analyzer,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<QueryResponse<ArangoVoid>>(database, HttpMethod.Post,
                ApiPath(database, "analyzer"),
                analyzer,
                cancellationToken: cancellationToken);
        }

        public async ValueTask DeleteAsync(ArangoHandle database,
            string analyzer, bool force = false,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<QueryResponse<ArangoVoid>>(database, HttpMethod.Delete,
                ApiPath(database, $"analyzer/{UrlEncode(analyzer)}?force={(force ? "true" : "false")}"),
                cancellationToken: cancellationToken);
        }
    }
}