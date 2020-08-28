using System;
using System.Collections.Generic;
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
        [Obsolete("use ArangoContext.Document.CreateAsync or ImportAsync")]
        public async Task CreateDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            bool bulk = false,
            CancellationToken cancellationToken = default) where T : class
        {
            if (bulk)
                await Document.ImportAsync(database, collection, docs, true, cancellationToken);
            else
                await Document.CreateAsync(database, collection, docs, waitForSync, silent, overwrite,
                    cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.CreateAsync")]
        public async Task<T> CreateDocumentAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            CancellationToken cancellationToken = default) where T : class
        {
            return await Document.CreateAsync(database, collection, doc, waitForSync, silent, overwrite, cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.ListAsync")]
        public async Task<List<string>> ListGraphAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            return await Graph.ListAsync(database, cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.CreateAsync")]
        public async Task CreateGraphAsync(ArangoHandle database, ArangoGraph request,
            CancellationToken cancellationToken = default)
        {
            await Graph.CreateAsync(database, request, cancellationToken);
        }

        [Obsolete("use ArangoContext.Graph.DropAsync")]
        public async Task DropGraphAsync(ArangoHandle database, string name,
            CancellationToken cancellationToken = default)
        {
            await Graph.DropAsync(database, name, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.CreateAsync")]
        public async Task CreateCollectionAsync(ArangoHandle database, string collection, ArangoCollectionType type,
            CancellationToken cancellationToken = default)
        {
            await Collection.CreateAsync(database, collection, type, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.CreateAsync")]
        public async Task CreateCollectionAsync(ArangoHandle database, ArangoCollection collection,
            CancellationToken cancellationToken = default)
        {
            await Collection.CreateAsync(database, collection, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.TruncateAsync")]
        public async Task TruncateCollectionAsync(ArangoHandle database, string collection,
            CancellationToken cancellationToken = default)
        {
            await Collection.TruncateAsync(database, collection, cancellationToken);
        }

        [Obsolete("use ArangoContext.Collection.ListAsync")]
        public async Task<List<string>> ListCollectionsAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            return await Collection.ListAsync(database, cancellationToken);
        }

        [Obsolete]
        public async Task<JObject> ExecuteTransactionAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            return await Transaction.ExecuteAsync(database, request, cancellationToken);
        }

        [Obsolete]
        public async Task<ArangoHandle> BeginTransactionAsync(ArangoHandle database, ArangoTransaction request,
            CancellationToken cancellationToken = default)
        {
            return await Transaction.BeginAsync(database, request, cancellationToken);
        }

        [Obsolete]
        public async Task CommitTransactionAsync(ArangoHandle database,
            CancellationToken cancellationToken = default)
        {
            await Transaction.CommitAsync(database, cancellationToken);
        }

        [Obsolete]
        public async Task AbortTransactionAsync(ArangoHandle database, CancellationToken cancellationToken = default)
        {
            await Transaction.AbortAsync(database, cancellationToken);
        }

        [Obsolete]
        public async Task<List<ArangoUpdateResult<TR>>> DeleteDocumentsAsync<T, TR>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await Document.DeleteAsync<T, TR>(database, collection, docs, waitForSync, returnOld);
        }

        [Obsolete]
        public async Task<ArangoUpdateResult<TR>> DeleteDocumentAsync<TR>(ArangoHandle database, string collection,
            string key,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? silent = null,
            CancellationToken cancellationToken = default)
        {
            return await Document.DeleteAsync<TR>(database, collection, key, waitForSync, returnOld);
        }

        [Obsolete]
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

        [Obsolete]
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
            return await Document.UpdateAsync<T, TR>(database, collection, docs, waitForSync, keepNull, mergeObjects, returnOld, returnNew, silent, cancellationToken);
        }

        [Obsolete]
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
            return await Document.UpdateAsync<T>(database, collection, doc, waitForSync, keepNull, mergeObjects, returnOld, returnNew, silent, cancellationToken);
        }

        [Obsolete]
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
            return await Document.UpdateAsync<T, TR>(database, collection, doc, waitForSync, keepNull, mergeObjects, returnOld, returnNew, silent, cancellationToken);
        }

        [Obsolete]
        public async Task<List<ArangoUpdateResult<TR>>> ReplaceDocumentsAsync<T, TR>(ArangoHandle database,
    string collection, IEnumerable<T> docs,
    bool? waitForSync = null,
    bool? returnOld = null,
    bool? returnNew = null,
    CancellationToken cancellationToken = default) where T : class
        {
            return await Document.ReplaceAsync<T, TR>(database, collection, docs, waitForSync, returnOld, returnNew, cancellationToken);
        }

        [Obsolete]
        public async Task<List<ArangoUpdateResult<JObject>>> ReplaceDocumentsAsync<T>(ArangoHandle database,
            string collection, IEnumerable<T> docs,
            bool? waitForSync = null,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await Document.ReplaceAsync(database, collection, docs, waitForSync, returnOld, returnNew, cancellationToken);
        }

        [Obsolete]
        public async Task<ArangoUpdateResult<TR>> ReplaceDocumentAsync<T, TR>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await Document.ReplaceAsync<T, TR>(database, collection, doc, waitForSync, returnOld, returnNew, cancellationToken);
        }

        [Obsolete]
        public async Task<ArangoUpdateResult<JObject>> ReplaceDocumentAsync<T>(ArangoHandle database, string collection,
            T doc,
            bool waitForSync = false,
            bool? returnOld = null,
            bool? returnNew = null,
            CancellationToken cancellationToken = default) where T : class
        {
            return await Document.ReplaceAsync(database, collection, doc, waitForSync, returnOld, returnNew, cancellationToken);
        }
    }
}