using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango
{
    public class ArangoList<T> : List<T>
    {
        public int? FullCount { get; set; }
    }

    /// <summary>
    ///     Thread-Safe ArangoDB Context
    /// </summary>
    public partial class ArangoContext
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new ArangoContractResolver(),
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        };

        private readonly string _password;
        private readonly string _realm;

        private readonly string _server;
        private readonly string _user;

        private string _auth;


        public ArangoContext(string cs)
        {
            var builder = new DbConnectionStringBuilder {ConnectionString = cs};
            builder.TryGetValue("Server", out var s);
            builder.TryGetValue("Realm", out var r);
            builder.TryGetValue("User ID", out var u);
            builder.TryGetValue("Password", out var p);

            var server = s as string;
            var user = u as string;
            var password = p as string;
            var realm = r as string;

            if (string.IsNullOrWhiteSpace(server))
                throw new ArgumentException("Server invalid");

            if (string.IsNullOrWhiteSpace(realm))
                throw new ArgumentException("Realm invalid");

            if (string.IsNullOrWhiteSpace(user))
                throw new ArgumentException("User invalid");

            //_auth = "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user}:{password ?? ""}"));
            _realm = realm + "-";
            _server = server;
            _user = user;
            _password = password;
        }

        public int BatchSize { get; set; } = 500;

        public ILogger Logger { get; set; }

        /// <summary>
        ///     AQL filter expression with x as iterator
        /// </summary>
        public async Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, int limit = 1000) where T : new()
        {
            var filterExp = Parameterize(filter, out var parameter);

            return await QueryAsync<T>(database,
                $"FOR x IN {collection} FILTER {filterExp} LIMIT {limit} RETURN {projection ?? "x"}",
                parameter);
        }

        public async Task RefreshJwtAuth()
        {
            var res = await SendAsync<JObject>(HttpMethod.Post, $"{_server}/_open/auth",
                JsonConvert.SerializeObject(new
                {
                    username = _user,
                    password = _password ?? string.Empty
                }, JsonSerializerSettings), auth: false);

            var jwt = res.Value<string>("jwt");
            _auth = $"Bearer {jwt}";
        }

        public async Task<T> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null) where T : new()
        {
            var results = await FindAsync<T>(database, collection, filter, projection, 2);

            if (results.Count == 0)
                return default;
            return results.SingleOrDefault();
        }

        public async Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, FormattableString query,
            bool? cache = null)
            where T : new()
        {
            var queryExp = Parameterize(query, out var parameter);

            return await QueryAsync<T>(database, queryExp, parameter, cache);
        }

        public async Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null)
            where T : new()
        {
            query = query.Trim();
            var final = new ArangoList<T>();

            try
            {
                var firstResult = await SendAsync<QueryResponse<T>>(HttpMethod.Post,
                    $"{_server}/_db/{DbName(database)}/_api/cursor",
                    JsonConvert.SerializeObject(new QueryRequest
                    {
                        Query = query,
                        BindVars = bindVars,
                        BatchSize = BatchSize,
                        Cache = cache,
                        Options = new QueryRequestOptions
                        {
                            FullCount = fullCount
                        }
                    }, JsonSerializerSettings));

                final.AddRange(firstResult.Result);

                if (Logger != null)
                {
                    var stats = firstResult.Extra.GetValue("stats");

                    var scannedFull = stats.Value<long>("scannedFull");
                    var scannedIndex = stats.Value<long>("scannedIndex");
                    var writesExecuted = stats.Value<long>("writesExecuted");
                    var executionTime = stats.Value<double>("executionTime") * 1000.0;

                    var boundQuery = query;

                    foreach (var p in bindVars)
                        boundQuery = boundQuery.Replace("@" + p.Key, JsonConvert.SerializeObject(p.Value));

                    Logger?.LogDebug(
                        "QueryAsync\n{Query}\ntime:{executionTime:N2}ms writes:{writesExecuted} fullscan:{scannedFull} indexscan:{scannedIndex}",
                        boundQuery,
                        executionTime,
                        writesExecuted,
                        scannedFull,
                        scannedIndex);

                    /*Logger?.LogDebug("QueryAsync\n{Query}\nVars\n{BindVars}\ntime:{executionTime:N2}ms writes:{writesExecuted} fullscan:{scannedFull} indexscan:{scannedIndex}",
                        query, string.Join('\n', bindVars.Select(x => x.Key + " = " + JsonConvert.SerializeObject(x.Value))),
                        executionTime,
                        writesExecuted,
                        scannedFull,
                        scannedIndex);*/

                    if (executionTime > 100.0 && writesExecuted == 0)
                        Logger?.LogWarning($"Slow Query detected {executionTime:N3}ms");
                }

                if (fullCount.HasValue && fullCount.Value)
                    final.FullCount = firstResult.Extra.GetValue("stats").Value<int>("fullCount");

                if (!firstResult.HasMore)
                    return final;

                while (true)
                {
                    var res = await SendAsync<QueryResponse<T>>(HttpMethod.Put,
                        $"{_server}/_db/{DbName(database)}/_api/cursor/{firstResult.Id}");

                    if (res.Result?.Any() == true)
                        final.AddRange(res.Result);

                    if (!res.HasMore)
                        break;
                }


                return final;
            }
            catch (Exception)
            {
                Logger?.LogError("QueryAsync\n{Query}\nVars\n{BindVars}",
                    query, string.Join('\n', bindVars.Select(x => x.Key + " " + JsonConvert.SerializeObject(x.Value))));
                throw;
            }
        }

        public async Task CreateDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false,
            bool bulk = false) where T : class
        {
            if (bulk)
            {
                var query = AddQueryString($"{_server}/_db/{DbName(database)}/_api/import",
                    new Dictionary<string, string>
                    {
                        {"type", "array"},
                        {"complete", "true"},
                        {"overwrite", overwrite.ToString().ToLowerInvariant()},
                        {"collection", collection}
                    });

                var res = await SendAsync<JObject>(HttpMethod.Post, query,
                    JsonConvert.SerializeObject(docs, JsonSerializerSettings));
            }
            else
            {
                var query = AddQueryString(
                    $"{_server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                    new Dictionary<string, string>
                    {
                        {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                        {"silent", silent.ToString().ToLowerInvariant()},
                        {"overwrite", overwrite.ToString().ToLowerInvariant()}
                    });

                var res = await SendAsync<JArray>(HttpMethod.Post, query,
                    JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                    database.Transaction);

                if (res != null)
                    foreach (var r in res)
                        if (r.Value<bool>("error"))
                            throw new ArgumentException(res.ToString());
            }
        }

        public async Task<T> CreateDocumentAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false,
            bool silent = true,
            bool overwrite = false) where T : class
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"silent", silent.ToString().ToLowerInvariant()},
                    {"overwrite", overwrite.ToString().ToLowerInvariant()}
                });

            var res = await SendAsync<DocumentCreateResponse<T>>(HttpMethod.Post, query,
                JsonConvert.SerializeObject(doc, JsonSerializerSettings),
                database.Transaction);

            return doc;
        }

        public async Task ReplaceDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false) where T : class
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString()}
                });

            var res = await SendAsync<JArray>(HttpMethod.Put, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction);
        }

        public async Task ReplaceDocumentAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false) where T : class
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/database/document/{UrlEncoder.Default.Encode(collection)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString()}
                });

            var res = await SendAsync<JArray>(HttpMethod.Put, query,
                JsonConvert.SerializeObject(new List<T> {doc}, JsonSerializerSettings),
                database.Transaction);
        }

        public async Task UpdateDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false,
            bool keepNull = true,
            bool mergeObjects = true) where T : class
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"keepNull", keepNull.ToString().ToLowerInvariant()},
                    {"mergeObjects", mergeObjects.ToString().ToLowerInvariant()}
                });

            var res = await SendAsync<JArray>(HttpMethod.Patch, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction);
        }

        public async Task UpdateDocumentAsync<T>(ArangoHandle database, string collection, T doc,
            bool waitForSync = false,
            bool keepNull = true,
            bool mergeObjects = true) where T : class
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"keepNull", keepNull.ToString().ToLowerInvariant()},
                    {"mergeObjects", mergeObjects.ToString().ToLowerInvariant()}
                });

            var res = await SendAsync<JArray>(HttpMethod.Patch, query,
                JsonConvert.SerializeObject(new List<T> {doc}, JsonSerializerSettings),
                database.Transaction);
        }

        public async Task<int> DeleteDocumentsAsync<T>(ArangoHandle database, string collection, IEnumerable<T> docs,
            bool waitForSync = false) where T : class
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/document/{collection}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()}
                });

            await SendAsync<JArray>(HttpMethod.Delete, query,
                JsonConvert.SerializeObject(docs, JsonSerializerSettings),
                database.Transaction);

            return 1;
        }

        public async Task DeleteDocumentAsync(ArangoHandle database, string collection, string key,
            bool waitForSync = false, bool silent = true)
        {
            var query = AddQueryString(
                $"{_server}/_db/{DbName(database)}/_api/document/{UrlEncoder.Default.Encode(collection)}/{UrlEncoder.Default.Encode(key)}",
                new Dictionary<string, string>
                {
                    {"waitForSync", waitForSync.ToString().ToLowerInvariant()},
                    {"silent", silent.ToString().ToLowerInvariant()}
                });

            await SendAsync<JObject>(HttpMethod.Delete, query, transaction: database.Transaction);
        }

        public async Task<ArangoHandle> BeginTransactionAsync(ArangoHandle database, ArangoTransaction request)
        {
            var res = await SendAsync<JObject>(HttpMethod.Post,
                $"{_server}/_db/{DbName(database)}/_api/transaction/begin",
                JsonConvert.SerializeObject(request, JsonSerializerSettings));

            var transaction = res.GetValue("result").Value<string>("id");
            return new ArangoHandle(database, transaction);
        }

        public async Task CommitTransactionAsync(ArangoHandle database)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<JObject>(HttpMethod.Put,
                $"{_server}/_db/{DbName(database)}/_api/transaction/{database.Transaction}");
        }

        public async Task AbortTransactionAsync(ArangoHandle database)
        {
            if (string.IsNullOrWhiteSpace(database.Transaction))
                throw new ArangoException("no transaction handle");

            await SendAsync<JObject>(HttpMethod.Delete,
                $"{_server}/_db/{DbName(database)}/_api/transaction/{database.Transaction}");
        }
    }
}