using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoGraphModule : IArangoGraphModule
    {
        private readonly IArangoContext _context;

        internal ArangoGraphModule(IArangoContext context)
        {
            _context = context;
        }

        public async Task<List<string>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await _context.SendAsync<GraphResponse<JObject>>(HttpMethod.Get,
                $"{_context.Server}/_db/{_context.DbName(database)}/_api/gharial", cancellationToken: cancellationToken);
            return res.Graphs.Select(x => x.Value<string>("_key")).ToList();
        }

        public async Task CreateAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default)
        {
            await _context.SendAsync<JObject>(HttpMethod.Post,
                $"{_context.Server}/_db/{_context.DbName(database)}/_api/gharial",
                JsonConvert.SerializeObject(request), cancellationToken: cancellationToken);
        }

        public async Task DropAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default)
        {
            await _context.SendAsync<JObject>(HttpMethod.Delete,
                $"{_context.Server}/_db/{_context.DbName(database)}/_api/gharial/{UrlEncoder.Default.Encode(name)}",
                cancellationToken: cancellationToken);
        }
    }
}