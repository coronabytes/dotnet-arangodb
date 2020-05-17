using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public partial class ArangoContext : IArangoContext
    {
        public async Task<List<ArangoUpdateResult<TR>>> ReplaceDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            if (returnNew.HasValue)
                parameter.Add("returnNew", returnNew.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}", parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(HttpMethod.Put, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<JObject>>> ReplaceDocumentsAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await ReplaceDocumentsAsync<T, JObject>(database, collection, docs,
                waitForSync, returnOld, returnNew, cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> ReplaceDocumentAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await ReplaceDocumentsAsync<T, TR>(database, collection, new List<T> {doc},
                waitForSync, returnOld, returnNew, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<JObject>> ReplaceDocumentAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await ReplaceDocumentsAsync<T, JObject>(database, collection, new List<T> {doc},
                waitForSync, returnOld, returnNew, cancellationToken);

            return res.SingleOrDefault();
        }
    }
}