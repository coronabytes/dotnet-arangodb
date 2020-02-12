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
    public partial class ArangoContext
    {
        public async Task<List<ArangoUpdateResult<JObject>>> UpdateDocumentsAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await UpdateDocumentsAsync<T, JObject>(database, collection, docs, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> UpdateDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (keepNull.HasValue)
                parameter.Add("keepNull", keepNull.Value.ToString().ToLowerInvariant());

            if (mergeObjects.HasValue)
                parameter.Add("mergeObjects", mergeObjects.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            if (returnNew.HasValue)
                parameter.Add("returnNew", returnNew.Value.ToString().ToLowerInvariant());

            if (silent.HasValue)
                parameter.Add("silent", silent.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}", parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(HttpMethod.Patch, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<ArangoUpdateResult<JObject>> UpdateDocumentAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await UpdateDocumentsAsync<T, JObject>(database, collection,
                new List<T> {doc}, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<TR>> UpdateDocumentAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await UpdateDocumentsAsync<T, TR>(database, collection,
                new List<T> {doc}, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, cancellationToken);

            return res.SingleOrDefault();
        }
    }
}