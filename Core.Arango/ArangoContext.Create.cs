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
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="docs"></param>
        /// <param name="waitForSync"></param>
        /// <param name="silent"></param>
        /// <param name="overwrite">In bulk mode truncates collection!</param>
        /// <param name="bulk">Optimized insert</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task CreateDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            bool bulk = false,
            CancellationToken cancellationToken = default) where T : class
        {
            if (bulk)
            {
                var query = AddQueryString($"{Server}/_db/{DbName(database)}/_api/import",
                    new Dictionary<string, string>
                    {
                        {"type", "array"},
                        {"complete", "true"},
                        {"overwrite", overwrite.ToString().ToLowerInvariant()},
                        {"collection", collection}
                    });

                var res = await SendAsync<JObject>(HttpMethod.Post, query,
                    JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                    cancellationToken: cancellationToken);
            }
            else
            {
                var query = AddQueryString(
                    $"{Server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                    new Dictionary<string, string>
                    {
                        {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                        {"silent", silent.ToString().ToLowerInvariant()},
                        {"overwrite", overwrite.ToString().ToLowerInvariant()}
                    });

                var res = await SendAsync<JArray>(HttpMethod.Post, query,
                    JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                    database.Transaction, cancellationToken: cancellationToken);

                if (res != null)
                    foreach (var r in res)
                        if (r.Value<bool>("error"))
                            throw new ArgumentException(res.ToString());
            }
        }

        public async Task<T> CreateDocumentAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            CancellationToken cancellationToken = default) where T : class
        {
            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"silent", silent.ToString().ToLowerInvariant()},
                    {"overwrite", overwrite.ToString().ToLowerInvariant()}
                });

            var res = await SendAsync<DocumentCreateResponse<T>>(HttpMethod.Post, query,
                JsonConvert.SerializeObject(doc, JsonSerializerSettings),
                database.Transaction, cancellationToken: cancellationToken);

            return doc;
        }
    }
}