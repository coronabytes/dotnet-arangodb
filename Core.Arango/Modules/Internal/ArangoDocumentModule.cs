using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoDocumentModule : ArangoModule, IArangoDocumentModule
    {
        internal ArangoDocumentModule(IArangoContext context) : base(context)
        {
        }

        public async Task<T> GetAsync<T>(ArangoHandle database, string collection, string key,
            bool throwOnError = true,
            string ifMatch = null,
            string ifNoneMatch = null,
            CancellationToken cancellationToken = default)
        {
            var headers = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(ifMatch))
                headers.Add("If-Match", $"\"{ifMatch}\"");
            if (!string.IsNullOrEmpty(ifNoneMatch))
                headers.Add("If-None-Match", $"\"{ifNoneMatch}\"");

            return await SendAsync<T>(database, HttpMethod.Get, ApiPath(database, $"document/{UrlEncode(collection)}/{key}"),
                null, throwOnError, headers: headers, cancellationToken: cancellationToken);
        }

        public async Task<List<T>> GetManyAsync<T>(ArangoHandle database, string collection, IEnumerable<object> docs, bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string> {{"getonly", "true"}};
            
            if (ignoreRevs.HasValue)
                parameter.Add("ignoreRevs", ignoreRevs.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<T>>(database, HttpMethod.Put, query, docs, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> CreateManyAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs, bool? waitForSync = null,
            bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null,
            bool? silent = null, ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default)
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

            if (overwriteMode.HasValue)
                parameter.Add("overwriteMode", overwriteMode.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(database, HttpMethod.Post, query,
                docs, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<ArangoVoid>>> CreateManyAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs, bool? waitForSync = null,
            bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null,
            bool? silent = null, ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default)
        {
            return await CreateManyAsync<T, ArangoVoid>(database, collection, docs, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, overwriteMode, cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> CreateAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null,
            bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null,
            bool? silent = null, ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default)
        {
            var res = await CreateManyAsync<T, TR>(database, collection, new List<T> {doc}, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, overwriteMode, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<ArangoVoid>> CreateAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null, bool? keepNull = null,
            bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null, bool? silent = null,
            ArangoOverwriteMode? overwriteMode = null, CancellationToken cancellationToken = default) 
        {
            var res = await CreateManyAsync<T, ArangoVoid>(database, collection, new List<T> {doc}, waitForSync,
                keepNull, mergeObjects,
                returnOld, returnNew, silent, overwriteMode, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task ImportAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool complete = true,
            CancellationToken cancellationToken = default)
        {
            var query = AddQueryString(ApiPath(database, "import"),
                new Dictionary<string, string>
                {
                    {"type", "array"},
                    {"complete", complete.ToString().ToLowerInvariant()},
                    {"collection", collection}
                });

            await SendAsync<ArangoVoid>(database, HttpMethod.Post, query,
                docs,
                cancellationToken: cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> DeleteAsync<TR>(ArangoHandle database, string collection,
            string key,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? silent = null,
            string ifMatch = null,
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

            var headers = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(ifMatch))
                headers.Add("If-Match", $"\"{ifMatch}\"");

            return await SendAsync<ArangoUpdateResult<TR>>(database, HttpMethod.Delete, query, headers: headers, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> DeleteManyAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,  
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            if (ignoreRevs.HasValue)
                parameter.Add("ignoreRevs", ignoreRevs.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                ApiPath(database, $"document/{collection}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(database, HttpMethod.Delete, query,
                docs,
                cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<ArangoVoid>>> UpdateManyAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            return await UpdateManyAsync<T, ArangoVoid>(database, collection, docs, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, ignoreRevs, cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> UpdateManyAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
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

            if (ignoreRevs.HasValue)
                parameter.Add("ignoreRevs", ignoreRevs.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(database, HttpMethod.Patch, query,
                docs, cancellationToken: cancellationToken);
        }

        public async Task<ArangoUpdateResult<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var res = await UpdateManyAsync<T, ArangoVoid>(database, collection,
                new List<T> {doc}, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, ignoreRevs, cancellationToken);

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
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var res = await UpdateManyAsync<T, TR>(database, collection,
                new List<T> {doc}, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, ignoreRevs, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<List<ArangoUpdateResult<TR>>> ReplaceManyAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>();

            if (waitForSync.HasValue)
                parameter.Add("waitForSync", waitForSync.Value.ToString().ToLowerInvariant());

            if (returnOld.HasValue)
                parameter.Add("returnOld", returnOld.Value.ToString().ToLowerInvariant());

            if (returnNew.HasValue)
                parameter.Add("returnNew", returnNew.Value.ToString().ToLowerInvariant());

            if (ignoreRevs.HasValue)
                parameter.Add("ignoreRevs", ignoreRevs.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(
                ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(database, HttpMethod.Put, query,
                docs, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<ArangoVoid>>> ReplaceManyAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            return await ReplaceManyAsync<T, ArangoVoid>(database, collection, docs,
                waitForSync, returnOld, returnNew, ignoreRevs, cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var res = await ReplaceManyAsync<T, TR>(database, collection, new List<T> {doc},
                waitForSync, returnOld, returnNew, ignoreRevs, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? ignoreRevs = null,
            CancellationToken cancellationToken = default)
        {
            var res = await ReplaceManyAsync<T, ArangoVoid>(database, collection, new List<T> {doc},
                waitForSync, returnOld, returnNew, ignoreRevs, cancellationToken);

            return res.SingleOrDefault();
        }


        public async IAsyncEnumerable<List<T>> ExportAsync<T>(ArangoHandle database,
            string collection, bool? flush = null, int? flushWait = null, int? batchSize = null, int? ttl = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (database.Batches != null)
                throw new NotSupportedException("no batch support");

            var parameter = new Dictionary<string, string>
            {
                ["collection"] = collection
            };

            var query = AddQueryString(
                ApiPath(database, "export"), parameter);

            var firstResult = await SendAsync<QueryResponse<T>>(database, HttpMethod.Post,
                query,
                new ExportRequest
                {
                    Flush = flush,
                    FlushWait = flushWait,
                    BatchSize = batchSize ?? Context.Configuration.BatchSize,
                    Ttl = ttl
                }, cancellationToken: cancellationToken);

            yield return firstResult.Result;

            if (firstResult.HasMore)
            {
                while (true)
                {
                    var res = await SendAsync<QueryResponse<T>>(database, HttpMethod.Put,
                        ApiPath(database, $"cursor/{firstResult.Id}"),
                        cancellationToken: cancellationToken);

                    yield return res.Result;

                    if (!res.HasMore)
                        break;
                }

                try
                {
                    await SendAsync<ArangoVoid>(database, HttpMethod.Delete,
                        ApiPath(database, $"cursor/{firstResult.Id}"),
                        cancellationToken: cancellationToken);
                }
                catch
                {
                    //
                }
            }
        }
    }
}