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
    internal class ArangoGraphModule : ArangoModule, IArangoGraphModule
    {
        internal ArangoGraphModule(IArangoContext context) : base(context)
        {
        }

        public async Task<List<string>> ListAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            var res = await SendAsync<GraphResponse<JObject>>(HttpMethod.Get,
                ApiPath(database, "gharial"), cancellationToken: cancellationToken);
            return res.Graphs.Select(x => x.Value<string>("_key")).ToList();
        }

        public async Task CreateAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Post,
                ApiPath(database, "gharial"),
                JsonConvert.SerializeObject(request), cancellationToken: cancellationToken);
        }

        public async Task DropAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default)
        {
            await SendAsync<JObject>(HttpMethod.Delete,
                ApiPath(database, $"gharial/{UrlEncode(name)}"),
                cancellationToken: cancellationToken);
        }
    }
}