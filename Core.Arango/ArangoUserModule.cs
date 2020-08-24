using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public class ArangoUserModule
    {
        private readonly ArangoContext _context;

        internal ArangoUserModule(ArangoContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Create a new user
        /// </summary>
        public async Task<bool> CreateAsync(ArangoUser user, CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<JObject>(HttpMethod.Post,
                $"{_context.Server}/_api/user",
                JsonConvert.SerializeObject(user), cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     List available users
        /// </summary>
        public async Task<IReadOnlyCollection<ArangoUser>> ListAsync(CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<QueryResponse<ArangoUser>>(HttpMethod.Get,
                $"{_context.Server}/_api/user", cancellationToken: cancellationToken);
            return res.Result.AsReadOnly();
        }

        /// <summary>
        ///     Set the database access level
        /// </summary>
        public async Task<bool> SetDatabaseAccessAsync(ArangoHandle handle, string user, ArangoAccess access,
            CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<JObject>(HttpMethod.Put,
                $"{_context.Server}/_api/user/{UrlEncoder.Default.Encode(user)}/database/{_context.DbName(handle)}",
                JsonConvert.SerializeObject(new
                {
                    grant = access
                }), cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     Clear the database access level, revert back to the default access level
        /// </summary>
        public async Task<bool> DeleteDatabaseAccessAsync(ArangoHandle handle, string user,
            CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<JObject>(HttpMethod.Delete,
                $"{_context.Server}/_api/user/{user}/database/{_context.DbName(handle)}",
                cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     Modify attributes of an existing user
        /// </summary>
        public async Task<bool> PatchAsync(ArangoUser user, CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<JObject>(HttpMethod.Patch,
                $"{_context.Server}/_api/user/{UrlEncoder.Default.Encode(user.Name)}",
                JsonConvert.SerializeObject(user),
                cancellationToken: cancellationToken);

            return res != null;
        }

        /// <summary>
        ///     Delete a user permanently.
        /// </summary>
        public async Task<bool> DeleteAsync(string user, CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<JObject>(HttpMethod.Delete,
                $"{_context.Server}/_api/user/{UrlEncoder.Default.Encode(user)}", cancellationToken: cancellationToken);

            return res != null;
        }
    }
}