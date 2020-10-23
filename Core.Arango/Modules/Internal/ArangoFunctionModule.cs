using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;
using Newtonsoft.Json;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoFunctionModule : ArangoModule, IArangoFunctionModule
    {
        private const string API = "aqlfunction";

        internal ArangoFunctionModule(IArangoContext context) : base(context)
        {
        }

        public async Task<bool> CreateAsync(ArangoHandle database, ArangoFunctionDefinition request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var res = await SendAsync<FunctionCreateResponse>(HttpMethod.Post, ApiPath(database, API),
                JsonConvert.SerializeObject(request), cancellationToken: cancellationToken);

            return res.IsNewlyCreated;
        }

        public async Task<int> RemoveAsync(ArangoHandle database, string name, bool? group = false,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<FunctionRemoveResponse>(HttpMethod.Delete,
                ApiPath(database, $"{API}/{name}?group={(group ?? false).ToString().ToLowerInvariant()}"),
                cancellationToken: cancellationToken);

            return res.DeletedCount;
        }

        public async Task<IReadOnlyCollection<ArangoFunctionDefinition>> ListAsync(ArangoHandle database,
            string ns = null,
            CancellationToken cancellationToken = default)
        {
            ns = string.IsNullOrEmpty(ns) ? "" : "/" + ns;

            var res = await SendAsync<QueryResponse<ArangoFunctionDefinition>>(HttpMethod.Get,
                ApiPath(database, API + ns), cancellationToken: cancellationToken);

            return res.Result;
        }
    }
}