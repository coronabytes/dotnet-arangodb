using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol.Internal;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoQueryModule : ArangoModule, IArangoQueryModule
    {
        internal ArangoQueryModule(IArangoContext context) : base(context)
        {
        }

        public async Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, int limit = 1000, CancellationToken cancellationToken = default)
        {
            var filterExp = Parameterize(filter, out var parameter);

            return await ExecuteAsync<T>(database,
                $"FOR x IN {collection} FILTER {filterExp} LIMIT {limit} RETURN {projection ?? "x"}",
                parameter, cancellationToken: cancellationToken);
        }

        public async Task<T> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, CancellationToken cancellationToken = default)
        {
            var results = await FindAsync<T>(database, collection, filter, projection, 2, cancellationToken);

            if (results.Count == 0)
                return default;
            return results.SingleOrDefault();
        }

        public async Task<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, FormattableString query,
            bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default)
        {
            var queryExp = Parameterize(query, out var parameter);

            return await ExecuteAsync<T>(database, queryExp, parameter, cache, fullCount, cancellationToken);
        }

        public async Task<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
        {
            query = query.Trim();
            var final = new ArangoList<T>();

            try
            {
                var firstResult = await SendAsync<QueryResponse<T>>(HttpMethod.Post,
                    ApiPath(database, "cursor"),
                    new QueryRequest
                    {
                        Query = query,
                        BindVars = bindVars,
                        BatchSize = Context.Configuration.BatchSize,
                        Cache = cache,
                        Options = new QueryRequestOptions
                        {
                            FullCount = fullCount
                        }
                    }, cancellationToken: cancellationToken);

                final.AddRange(firstResult.Result);

                Context.Configuration.QueryProfile?.Invoke(query, bindVars, firstResult.Extra.Statistic);

                if (fullCount.HasValue && fullCount.Value)
                    final.FullCount = firstResult.Extra.Statistic.FullCount;

                if (!firstResult.HasMore)
                    return final;

                while (true)
                {
                    var res = await SendAsync<QueryResponse<T>>(HttpMethod.Put,
                        ApiPath(database, $"/cursor/{firstResult.Id}"),
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
                Context.Configuration.QueryProfile?.Invoke(query, bindVars, null);
                throw;
            }
        }

        public async Task<object> ExecuteAsync(Type type, bool isEnumerable, ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
        {
            query = query.Trim();

            var responseType = typeof(QueryResponse<>);
            var constructedResponseType = responseType.MakeGenericType(type);

            var body = new QueryRequest
            {
                Query = query,
                BindVars = bindVars,
                BatchSize = Context.Configuration.BatchSize,
                Cache = cache,
                Options = new QueryRequestOptions
                {
                    FullCount = fullCount
                }
            };

            var res = await SendAsync(constructedResponseType, HttpMethod.Post,
                ApiPath(database, "cursor"), body
                , cancellationToken: cancellationToken);

            var listResult = constructedResponseType.GetProperty("Result").GetValue(res);

            if (isEnumerable)
                return listResult;

            var count = (int) listResult.GetType().GetProperty("Count").GetValue(listResult);

            if (count == 0)
                return Activator.CreateInstance(type);

            return listResult.GetType().GetProperty("Item").GetValue(listResult, new object[] {0});
        }

        public IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, FormattableString query, bool? cache = null, 
            int? batchSize = null, CancellationToken cancellationToken = default)
        {
            var queryExp = Parameterize(query, out var parameter);
            return ExecuteStreamAsync<T>(database, queryExp, parameter, cache, batchSize, cancellationToken);
        }

        public async IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, int? batchSize = null,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            query = query.Trim();

            var firstResult = await SendAsync<QueryResponse<T>>(HttpMethod.Post,
                ApiPath(database, "cursor"),
                new QueryRequest
                {
                    Query = query,
                    BindVars = bindVars,
                    BatchSize = batchSize ?? Context.Configuration.BatchSize,
                    Cache = cache
                }, cancellationToken: cancellationToken);

            Context.Configuration.QueryProfile?.Invoke(query, bindVars, firstResult.Extra.Statistic);

            foreach (var result in firstResult.Result)
                yield return result;

            if (!firstResult.HasMore)
                yield break;

            while (true)
            {
                var res = await SendAsync<QueryResponse<T>>(HttpMethod.Put,
                    ApiPath(database, $"/cursor/{firstResult.Id}"),
                    cancellationToken: cancellationToken);

                if (res.Result?.Any() == true)
                    foreach (var result in firstResult.Result)
                        yield return result;

                if (!res.HasMore)
                    break;
            }
        }
    }
}