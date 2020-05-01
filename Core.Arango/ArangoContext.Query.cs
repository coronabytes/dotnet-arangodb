using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        ///     AQL filter expression with x as iterator
        /// </summary>
        public async Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, int limit = 1000, CancellationToken cancellationToken = default) where T : new()
        {
            var filterExp = Parameterize(filter, out var parameter);

            return await QueryAsync<T>(database,
                $"FOR x IN {collection} FILTER {filterExp} LIMIT {limit} RETURN {projection ?? "x"}",
                parameter, cancellationToken: cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, CancellationToken cancellationToken = default) where T : new()
        {
            var results = await FindAsync<T>(database, collection, filter, projection, 2, cancellationToken);

            if (results.Count == 0)
                return default;
            return results.SingleOrDefault();
        }

        public async Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, FormattableString query,
            bool? cache = null, CancellationToken cancellationToken = default)
            where T : new()
        {
            var queryExp = Parameterize(query, out var parameter);

            return await QueryAsync<T>(database, queryExp, parameter, cache, cancellationToken: cancellationToken);
        }

        public async Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
            where T : new()
        {
            query = query.Trim();
            var final = new ArangoList<T>();

            try
            {
                var firstResult = await SendAsync<QueryResponse<T>>(HttpMethod.Post,
                    $"{Server}/_db/{DbName(database)}/_api/cursor",
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
                    }, JsonSerializerSettings), cancellationToken: cancellationToken);

                final.AddRange(firstResult.Result);

                QueryProfile?.Invoke(query, bindVars, firstResult.Extra.GetValue("stats"));

                /*var stats = firstResult.Extra.GetValue("stats");

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
                        scannedIndex);*/

                if (fullCount.HasValue && fullCount.Value)
                    final.FullCount = firstResult.Extra.GetValue("stats").Value<int>("fullCount");

                if (!firstResult.HasMore)
                    return final;

                while (true)
                {
                    var res = await SendAsync<QueryResponse<T>>(HttpMethod.Put,
                        $"{Server}/_db/{DbName(database)}/_api/cursor/{firstResult.Id}",
                        cancellationToken: cancellationToken);

                    if (res.Result?.Any() == true)
                        final.AddRange(res.Result);

                    if (!res.HasMore)
                        break;
                }


                return final;
            }
            catch
            {
                QueryProfile?.Invoke(query, bindVars, null);
                throw;
            }
        }

        /// <summary>
        ///  For Linq Provider
        /// </summary>
        public async Task<object> QueryAsync(Type type, bool isEnumerable, ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
        {
            query = query.Trim();

            var responseType = typeof(QueryResponse<>);
            var constructedResponseType = responseType.MakeGenericType(type);

            var res = await SendAsync(constructedResponseType, HttpMethod.Post,
                $"{Server}/_db/{DbName(database)}/_api/cursor",
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
                }, JsonSerializerSettings), cancellationToken: cancellationToken);

            var listResult = constructedResponseType.GetProperty("Result").GetValue(res);

            if (isEnumerable)
                return listResult;

            var count = (int) listResult.GetType().GetProperty("Count").GetValue(listResult);

            if (count == 0)
                return Activator.CreateInstance(type);

            return listResult.GetType().GetProperty("Item").GetValue(listResult, new object[] {0});
        }

        /// <summary>
        ///     Note: this API is currently not supported on cluster coordinators.
        /// </summary>
        public async IAsyncEnumerable<List<JObject>> ExportAsync(ArangoHandle database,
            string collection, bool? flush = null, int? flushWait = null, int? batchSize = null, int? ttl = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var parameter = new Dictionary<string, string>
            {
                ["collection"] = collection
            };

            var query = AddQueryString(
                $"{Server}/_db/{DbName(database)}/_api/export", parameter);

            var firstResult = await SendAsync<QueryResponse<JObject>>(HttpMethod.Post,
                query,
                JsonConvert.SerializeObject(new ExportRequest
                {
                    Flush = flush,
                    FlushWait = flushWait,
                    BatchSize = batchSize,
                    Ttl = ttl
                }, JsonSerializerSettings), cancellationToken: cancellationToken);

            yield return firstResult.Result;

            if (firstResult.HasMore)
            {
                while (true)
                {
                    var res = await SendAsync<QueryResponse<JObject>>(HttpMethod.Put,
                        $"{Server}/_db/{DbName(database)}/_api/cursor/{firstResult.Id}",
                        cancellationToken: cancellationToken);

                    yield return res.Result;

                    if (!res.HasMore)
                        break;
                }

                try
                {
                    await SendAsync<JObject>(HttpMethod.Delete,
                        $"{Server}/_db/{DbName(database)}/_api/cursor/{firstResult.Id}",
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