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
    internal class ArangoQueryModule : ArangoModule, IArangoQueryModule
    {
        internal ArangoQueryModule(IArangoContext context) : base(context)
        {
        }

        public async ValueTask<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string? projection = null, int limit = 1000, CancellationToken cancellationToken = default)
        {
            var filterExp = Parameterize(filter, out var parameter);

            return await ExecuteAsync<T>(database,
                $"FOR x IN {collection} FILTER {filterExp} LIMIT {limit} RETURN {projection ?? "x"}",
                parameter,
                null,
                null,
                cancellationToken).ConfigureAwait(false);
        }

        public async ValueTask<T?> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string? projection = null, CancellationToken cancellationToken = default)
        {
            var results = await FindAsync<T>(database, collection, filter, projection, 2, cancellationToken)
                .ConfigureAwait(false);

            if (results.Count == 0)
                return default;
            return results.SingleOrDefault();
        }

        #region ExecuteWithoutState

        public ValueTask<object> ExecuteAsync(Type type, bool isEnumerable, ArangoHandle database, string query, IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync(type, isEnumerable, database, query, bindVars, null, cache, fullCount, cancellationToken);
        }

        public ValueTask<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, FormattableString query, bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync<T>(database, query, null, cache, fullCount, cancellationToken);
        }

        public ValueTask<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, string query, IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default)
        {
            return ExecuteAsync<T>(database, query, bindVars, null, cache, fullCount, cancellationToken);
        }

        public IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, FormattableString query, bool? cache = null, int? batchSize = null, bool? lazy = null, bool? fillBlockCache = null, long? memoryLimit = null, CancellationToken cancellationToken = default)
        {
            return ExecuteStreamAsync<T>(database, query, null, cache, batchSize, lazy, fillBlockCache, memoryLimit, cancellationToken);
        }

        public IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, string query, IDictionary<string, object> bindVars, bool? cache = null, int? batchSize = null, double? ttl = null, long? memoryLimit = null, ArangoQueryOptions? options = null, CancellationToken cancellationToken = default)
        {
            return ExecuteStreamAsync<T>(database, query, bindVars, null, cache, batchSize, ttl, memoryLimit, options, cancellationToken);
        }

        public IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, ArangoCursor cursor, CancellationToken cancellationToken = default)
        {
            return ExecuteStreamAsync<T>(database, cursor, null, cancellationToken);
        }

        #endregion

        #region ExecuteWithoutState

        public async ValueTask<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, FormattableString query,
            object? state, bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default)
        {
            var queryExp = Parameterize(query, out var parameter);

            return await ExecuteAsync<T>(database, queryExp, parameter, state, cache, fullCount, cancellationToken)
                .ConfigureAwait(false);
        }

        public async ValueTask<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, string query,
        IDictionary<string, object> bindVars, object? state, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
        {
            query = query.Trim();
            var final = new ArangoList<T>();

            try
            {
                var firstResult = await SendAsync<QueryResponse<T>>(database, HttpMethod.Post,
                    ApiPath(database, "cursor"),
                    new ArangoCursor
                    {
                        Query = query,
                        BindVars = bindVars,
                        BatchSize = Context.Configuration.BatchSize,
                        Cache = cache,
                        Options = new ArangoQueryOptions
                        {
                            FullCount = fullCount
                        }
                    }, cancellationToken: cancellationToken).ConfigureAwait(false);

                final.AddRange(firstResult.Result);

                if (fullCount.HasValue && fullCount.Value)
                    final.FullCount = firstResult.Extra.Statistic.FullCount;

                if (firstResult.HasMore)
                {
                    QueryResponse<T> lastResponse;

                    do
                    {
                        lastResponse = await SendAsync<QueryResponse<T>>(database, HttpMethod.Post,
                            ApiPath(database, $"/cursor/{firstResult.Id}"),
                            cancellationToken: cancellationToken).ConfigureAwait(false);

                        if (lastResponse.Result?.Any() == true)
                            final.AddRange(lastResponse.Result);

                    } while (lastResponse.HasMore);
                }

                Context.Configuration.QueryProfile?.Invoke(query, bindVars, firstResult.Extra.Statistic);
                Context.Configuration.QueryProfileWithState?.Invoke(query, bindVars, firstResult.Extra.Statistic, final.Count, state);

                return final;
            }
            catch
            {
                Context.Configuration.QueryProfile?.Invoke(query, bindVars, null);
                Context.Configuration.QueryProfileWithState?.Invoke(query, bindVars, null, -1, state);

                throw;
            }
        }

        public async ValueTask<object> ExecuteAsync(Type type, bool isEnumerable, ArangoHandle database, string query,
            IDictionary<string, object> bindVars, object? state = null, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
        {
            query = query.Trim();

            var responseType = typeof(QueryResponse<>);
            var constructedResponseType = responseType.MakeGenericType(type);

            var body = new ArangoCursor
            {
                Query = query,
                BindVars = bindVars,
                BatchSize = Context.Configuration.BatchSize,
                Cache = cache,
                Options = new ArangoQueryOptions
                {
                    FullCount = fullCount
                }
            };

            var res = await SendAsync(constructedResponseType, HttpMethod.Post,
                ApiPath(database, "cursor"), body, database.Transaction,
                cancellationToken: cancellationToken).ConfigureAwait(false);

            var listResult = constructedResponseType.GetProperty("Result").GetValue(res);

            if (isEnumerable)
                return listResult;

            var count = listResult.GetType().GetProperty("Count").GetValue(listResult);

            if (count is int i && i == 0)
                return Activator.CreateInstance(type);
            if (count is long l && l == 0)
                return Activator.CreateInstance(type);

            return listResult.GetType().GetProperty("Item").GetValue(listResult, new object[] { 0 });
        }

        public IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, FormattableString query,
            object? state = null, bool? cache = null, int? batchSize = null,
            bool? lazy = null, bool? fillBlockCache = null, long? memoryLimit = null,
            CancellationToken cancellationToken = default)
        {
            var queryExp = Parameterize(query, out var parameter);
            return ExecuteStreamAsync<T>(database, queryExp, parameter, state, cache, batchSize, null, memoryLimit,
                new ArangoQueryOptions
                {
                    Stream = lazy,
                    FillBlockCache = fillBlockCache
                }, cancellationToken);
        }

        public IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, object? state = null, bool? cache = null, int? batchSize = null,
            double? ttl = null, long? memoryLimit = null,
            ArangoQueryOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            query = query.Trim();

            return ExecuteStreamAsync<T>(database, new ArangoCursor
            {
                Query = query,
                BindVars = bindVars,
                BatchSize = batchSize ?? Context.Configuration.BatchSize,
                TTL = ttl,
                MemoryLimit = memoryLimit,
                Cache = cache,
                Options = options
            }, state, cancellationToken);
        }

        public async IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, ArangoCursor cursor,
            object? state = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var firstResult = await SendAsync<QueryResponse<T>>(database, HttpMethod.Post,
                ApiPath(database, "cursor"),
                cursor, cancellationToken: cancellationToken).ConfigureAwait(false);

            Context.Configuration.QueryProfile?.Invoke(cursor.Query, cursor.BindVars, firstResult.Extra.Statistic);
            Context.Configuration.QueryProfileWithState?.Invoke(cursor.Query, cursor.BindVars, firstResult.Extra.Statistic, firstResult.Count, state);

            foreach (var result in firstResult.Result)
                yield return result;

            if (!firstResult.HasMore)
                yield break;

            while (true)
            {
                var res = await SendAsync<QueryResponse<T>>(database, HttpMethod.Post,
                    ApiPath(database, $"/cursor/{firstResult.Id}"),
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                if (res.Result?.Any() == true)
                    foreach (var result in res.Result)
                        yield return result;

                if (!res.HasMore)
                    break;
            }
        }

        #endregion

        public ValueTask<ArangoExplainResult> ExplainAsync(ArangoHandle database, string query,
            IDictionary<string, object> bindVars,
            bool allPlans = false,
            CancellationToken cancellationToken = default)
        {
            return SendAsync<ArangoExplainResult>(database, HttpMethod.Post, ApiPath(database, "explain"),
                new
                {
                    query,
                    bindVars,
                    options = new
                    {
                        allPlans
                    }
                }, cancellationToken: cancellationToken);
        }

        public ValueTask<ArangoExplainResult> ExplainAsync(ArangoHandle database, FormattableString query,
            bool allPlans = false,
            CancellationToken cancellationToken = default)
        {
            var queryExp = Parameterize(query, out var parameter);

            return ExplainAsync(database, queryExp, parameter, allPlans, cancellationToken);
        }

        public ValueTask<ArangoParseResult> ParseAsync(ArangoHandle database, string query,
            CancellationToken cancellationToken = default)
        {
            return SendAsync<ArangoParseResult>(database, HttpMethod.Post, ApiPath(database, "query"),
                new
                {
                    query
                }, cancellationToken: cancellationToken);
        }
    }
}