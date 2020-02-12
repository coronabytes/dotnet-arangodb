using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
                        $"{Server}/_db/{DbName(database)}/_api/cursor/{firstResult.Id}",
                        cancellationToken: cancellationToken);

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
    }
}