using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        public async Task<int> DeleteDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            CancellationToken cancellationToken = default) where T : class
        {
            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{collection}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()}
                });

            await SendAsync<JArray>(HttpMethod.Delete, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction, cancellationToken: cancellationToken);

            return 1;
        }

        public async Task DeleteDocumentAsync(ArangoHandle database, string collection, string key,
            bool waitForSync = false, bool silent = true,
            CancellationToken cancellationToken = default)
        {
            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}/{UrlEncoder.Default.Encode(key)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"silent", silent.ToString().ToLowerInvariant()}
                });

            await SendAsync<JObject>(HttpMethod.Delete, query, transaction: database.Transaction,
                cancellationToken: cancellationToken);
        }
    }
}