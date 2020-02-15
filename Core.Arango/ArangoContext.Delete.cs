using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;

namespace Core.Arango
{
    public partial class ArangoContext
    {
        public async Task<List<ArangoUpdateResult<TR>>> DeleteDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{collection}", parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(HttpMethod.Delete, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> DeleteDocumentAsync<TR>(ArangoHandle database, string collection,
            string key,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? silent = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            if (silent.HasValue)
                parameter.Add("silent", silent.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}/{UrlEncoder.Default.Encode(key)}",
                parameter);

            return await SendAsync<ArangoUpdateResult<TR>>(HttpMethod.Delete, query, transaction: database.Transaction,
                cancellationToken: cancellationToken);
        }
    }
}