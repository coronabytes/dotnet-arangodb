using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;

namespace Core.Arango.Modules.Internal
{
    /// <summary>
    /// Implementation of user functions API https://www.arangodb.com/docs/stable/http/aql-user-functions.html
    /// </summary>
    internal class ArangoFunctionModule : ArangoModule, IArangoFunctionModule
    {
        private const string API = "aqlfunction";

        internal ArangoFunctionModule(IArangoContext context) : base(context)
        {
        }

        public Task<FunctionCreateResponse> CreateAsync(ArangoHandle database, ArangoFunctionDefinition request,
            CancellationToken cancellationToken = default)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return SendAsync<FunctionCreateResponse>(HttpMethod.Post, ApiPath(database, API),
                JsonConvert.SerializeObject(request), cancellationToken: cancellationToken);
        }

        public Task<FunctionRemoveResponse> RemoveAsync(ArangoHandle database, FunctionRemoveRequest request,
            CancellationToken cancellationToken = default)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var isGroup = request.Group.ToString().ToLower();

            return SendAsync<FunctionRemoveResponse>(HttpMethod.Delete,
                ApiPath(database, $"{API}/{request.Name}?group={isGroup}"), cancellationToken: cancellationToken);
        }

        public Task<FunctionListResponse> ListAsync(ArangoHandle database, FunctionListRequest request = default,
            CancellationToken cancellationToken = default)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));

            var @namespace = string.IsNullOrEmpty(request?.Namespace) ? "" : "/" + request.Namespace;

            return SendAsync<FunctionListResponse>(HttpMethod.Get,
                ApiPath(database, API + @namespace), cancellationToken: cancellationToken);
        }
    }
}