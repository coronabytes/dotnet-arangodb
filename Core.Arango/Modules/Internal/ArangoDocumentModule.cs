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
            CancellationToken cancellationToken = default) where T : class
        {
            return await SendAsync<T>(HttpMethod.Get, ApiPath(database, $"document/{UrlEncode(collection)}/{key}"),
                null, database.Transaction, throwOnError, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> CreateManyAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs, bool? waitForSync = null,
            bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null,
            bool? silent = null, ArangoOverwriteMode? overwriteMode = null,
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

            if (overwriteMode.HasValue)
                parameter.Add("overwriteMode", overwriteMode.Value.ToString().ToLowerInvariant());

            var query = AddQueryString(ApiPath(database, $"document/{UrlEncode(collection)}"), parameter);

            return await SendAsync<List<ArangoUpdateResult<TR>>>(database, HttpMethod.Post, query,
                docs,
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<ArangoVoid>>> CreateManyAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs, bool? waitForSync = null,
            bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null,
            bool? silent = null, ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await CreateManyAsync<T, ArangoVoid>(database, collection, docs, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, overwriteMode, cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> CreateAsync<T, TR>(ArangoHandle database, string collection, T doc,
            bool? waitForSync = null,
            bool? keepNull = null, bool? mergeObjects = null, bool? returnOld = null, bool? returnNew = null,
            bool? silent = null, ArangoOverwriteMode? overwriteMode = null,
            CancellationToken cancellationToken = default) where T : class
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
            ArangoOverwriteMode? overwriteMode = null, CancellationToken cancellationToken = default) where T : class
        {
            var res = await CreateManyAsync<T, ArangoVoid>(database, collection, new List<T> {doc}, waitForSync,
                keepNull, mergeObjects,
                returnOld, returnNew, silent, overwriteMode, cancellationToken);

            return res.SingleOrDefault();
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

            await SendAsync<ArangoVoid>(HttpMethod.Post, query,
                docs,
                cancellationToken: cancellationToken);
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

        public async Task<List<ArangoUpdateResult<TR>>> DeleteManyAsync<T, TR>(ArangoHandle database,
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
                docs,
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<ArangoVoid>>> UpdateManyAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await UpdateManyAsync<T, ArangoVoid>(database, collection, docs, waitForSync, keepNull,
                mergeObjects,
                returnOld, returnNew, silent, cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<TR>>> UpdateManyAsync<T, TR>(ArangoHandle database,
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
                docs,
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<ArangoUpdateResult<ArangoVoid>> UpdateAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool? waitForSync = null,
            bool? keepNull = null,
            bool? mergeObjects = null,
            bool? returnOld = null,
            bool? returnNew = null,
            bool? silent = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await UpdateManyAsync<T, ArangoVoid>(database, collection,
                new List<T> {doc}, waitForSync, keepNull, mergeObjects,
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
            var res = await UpdateManyAsync<T, TR>(database, collection,
                new List<T> {doc}, waitForSync, keepNull, mergeObjects,
                returnOld, returnNew, silent, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<List<ArangoUpdateResult<TR>>> ReplaceManyAsync<T, TR>(ArangoHandle database,
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
                docs,
                database.Transaction, cancellationToken: cancellationToken);
        }

        public async Task<List<ArangoUpdateResult<ArangoVoid>>> ReplaceManyAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await ReplaceManyAsync<T, ArangoVoid>(database, collection, docs,
                waitForSync, returnOld, returnNew, cancellationToken);
        }

        public async Task<ArangoUpdateResult<TR>> ReplaceAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await ReplaceManyAsync<T, TR>(database, collection, new List<T> {doc},
                waitForSync, returnOld, returnNew, cancellationToken);

            return res.SingleOrDefault();
        }

        public async Task<ArangoUpdateResult<ArangoVoid>> ReplaceAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            var res = await ReplaceManyAsync<T, ArangoVoid>(database, collection, new List<T> {doc},
                waitForSync, returnOld, returnNew, cancellationToken);

            return res.SingleOrDefault();
        }


        public async IAsyncEnumerable<List<T>> ExportAsync<T>(ArangoHandle database,
            string collection, bool? flush = null, int? flushWait = null, int? batchSize = null, int? ttl = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>
            {
                ["collection"] = collection
            };

            var query = AddQueryString(
                ApiPath(database, "export"), parameter);

            var firstResult = await SendAsync<QueryResponse<T>>(HttpMethod.Post,
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
                    var res = await SendAsync<QueryResponse<T>>(HttpMethod.Put,
                        ApiPath(database, $"cursor/{firstResult.Id}"),
                        cancellationToken: cancellationToken);

                    yield return res.Result;

                    if (!res.HasMore)
                        break;
                }

                try
                {
                    await SendAsync<ArangoVoid>(HttpMethod.Delete,
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