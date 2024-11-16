using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoBackupModule : ArangoModule, IArangoBackupModule
    {
        internal ArangoBackupModule(IArangoContext context) : base(context)
        {
        }

        public async ValueTask<ArangoBackup> CreateAsync(ArangoBackupRequest request,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<SingleResult<ArangoBackup>>(null, HttpMethod.Post, "/_admin/backup/create",
                request, cancellationToken: cancellationToken);

            return res.Result;
        }

        public async ValueTask RestoreAsync(string id, CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(null, HttpMethod.Post, "/_admin/backup/restore",
                new
                {
                    id
                }, cancellationToken: cancellationToken);
        }

        public async ValueTask DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            await SendAsync<ArangoVoid>(null, HttpMethod.Post, "/_admin/backup/delete",
                new
                {
                    id
                }, cancellationToken: cancellationToken);
        }

        public async ValueTask<List<ArangoBackup>> ListAsync(string id = null, CancellationToken cancellationToken = default)
        {
            object req = null;

            if (id != null)
                req = new { id };

            var res = await SendAsync<SingleResult<BackupList>>(null, HttpMethod.Post, "/_admin/backup/list",
                req, cancellationToken: cancellationToken);

            return res.Result.List.Values.ToList();
        }
    }
}