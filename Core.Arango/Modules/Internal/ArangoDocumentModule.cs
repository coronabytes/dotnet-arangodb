using System;
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
    internal class ArangoDocumentModule : ArangoModule, IArangoDocumentModule
    {
        internal ArangoDocumentModule(IArangoContext context) : base(context)
        {
        }

        public async Task CreateAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            CancellationToken cancellationToken = default) where T : class
        {
            var query = AddQueryString(
                ApiPath(database, $"document/{UrlEncode(collection)}"),
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"silent", silent.ToString().ToLowerInvariant()},
                    {"overwrite", overwrite.ToString().ToLowerInvariant()}
                });

            var res = await SendAsync<JArray>(HttpMethod.Post, query,
                Serialize(docs),
                database.Transaction, cancellationToken: cancellationToken);

            if (res != null)
                foreach (var r in res)
                    if (r.Value<bool>("error"))
                        throw new ArgumentException(res.ToString());

        }

        public async Task ImportAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool complete = true,
            CancellationToken cancellationToken = default) where T : class
        {

            var query = AddQueryString(ApiPath(database, "import"),
                new Dictionary<string, string>
                {
                        {"type", "array"},
                        {"complete", complete.ToString().ToLowerInvariant()},
                        {"collection", collection}
                });

            var res = await SendAsync<JObject>(HttpMethod.Post, query,
                Serialize(docs),
                cancellationToken: cancellationToken);
        }

        public async Task<T> CreateAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            CancellationToken cancellationToken = default) where T : class
        {
            var query = AddQueryString(
                ApiPath(database, $"document/{UrlEncode(collection)}"),
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"silent", silent.ToString().ToLowerInvariant()},
                    {"overwrite", overwrite.ToString().ToLowerInvariant()}
                });

            var res = await SendAsync<DocumentCreateResponse<T>>(HttpMethod.Post, query,
                Serialize(doc),
                database.Transaction, cancellationToken: cancellationToken);

            return doc;
        }

        public async Task<ArangoUpdateResult<TR>> DeleteAsync<TR>(ArangoHandle database, string collection,
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
                ApiPath(database, $"document/{UrlEncode(collection)}/{UrlEncode(key)}"),
                parameter);

            return await SendAsync<ArangoUpdateResult<TR>>(HttpMethod.Delete, query, transaction: database.Transaction,
                cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> DeleteAsync<T, TR>(ArangoHandle database,
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
                ApiPath(database, $"document/{collection}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(HttpMethod.Delete, query,
                Serialize(docs),
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<JObject>>> UpdateAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await UpdateAsync<T, JObject>(database, collection, docs, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> UpdateAsync<T, TR>(ArangoHandle database,
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
                ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(HttpMethod.Patch, query,
                Serialize(docs),
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<ArangoUpdateResult<JObject>> UpdateAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await UpdateAsync<T, JObject>(database, collection,
                new List<T> { doc }, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<TR>> UpdateAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await UpdateAsync<T, TR>(database, collection,
                new List<T> { doc }, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<List<ArangoUpdateResult<TR>>> ReplaceAsync<T, TR>(ArangoHandle database,
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
                ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(HttpMethod.Put, query,
                Serialize(docs),
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<JObject>>> ReplaceAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await ReplaceAsync<T, JObject>(database, collection, docs,
                waitForSync, returnOld, returnNew, cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await ReplaceAsync<T, TR>(database, collection, new List<T> { doc },
                waitForSync, returnOld, returnNew, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<JObject>> ReplaceAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await ReplaceAsync<T, JObject>(database, collection, new List<T> { doc },
                waitForSync, returnOld, returnNew, cancellationToken);

            return res.SingleOrDefault();
        }
    }
}