using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoUserModule : ArangoModule, IArangoUserModule
    {
        internal ArangoUserModule(IArangoContext context) : base(context)
        {
        }

        /// <summary>
        ///     Create a new user
        /// </summary>
        public async ValueTask<bool> CreateAsync(ArangoUser user, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVoid>(null, HttpMethod.Post,
                ApiPath("user"), user, cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     List available users
        /// </summary>
        public async ValueTask<IReadOnlyCollection<ArangoUser>> ListAsync(CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<QueryResponse<ArangoUser>>(null, HttpMethod.Get,
                ApiPath("user"), cancellationToken: cancellationToken);
            return res.Result.AsReadOnly();
        }

        /// <summary>
        ///     Set the database access level
        /// </summary>
        public async ValueTask<bool> SetDatabaseAccessAsync(ArangoHandle handle, string user, ArangoAccess access,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVoid>(null, HttpMethod.Put,
                ApiPath($"user/{UrlEncode(user)}/database/{RealmPrefix(handle)}"),
                new
                {
                    grant = access
                }, cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     Clear the database access level, revert back to the default access level
        /// </summary>
        public async ValueTask<bool> DeleteDatabaseAccessAsync(ArangoHandle handle, string user,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVoid>(null, HttpMethod.Delete,
                ApiPath($"user/{UrlEncode(user)}/database/{RealmPrefix(handle)}"),
                cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     Modify attributes of an existing user
        /// </summary>
        public async ValueTask<bool> PatchAsync(ArangoUser user, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVoid>(null, PolyfillHelper.Patch,
                ApiPath($"user/{UrlEncode(user.Name)}"),
                user,
                cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     Delete a user permanently.
        /// </summary>
        public async ValueTask<bool> DeleteAsync(string user, CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<ArangoVoid>(null, HttpMethod.Delete,
                ApiPath($"user/{UrlEncode(user)}"), cancellationToken: cancellationToken);

            return res != null;
        }
    }
}