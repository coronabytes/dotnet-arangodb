using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules.Internal
{
    internal class ArangoQueryModule : ArangoModule, IArangoQueryModule
    {
        private readonly IArangoContext _context;

        internal ArangoQueryModule(IArangoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, int limit = 1000, CancellationToken cancellationToken = default) where T : new()
        {
            var filterExp = Parameterize(filter, out var parameter);

            return await ExecuteAsync<T>(database,
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

        public async Task<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, FormattableString query,
            bool? cache = null, CancellationToken cancellationToken = default)
            where T : new()
        {
            var queryExp = Parameterize(query, out var parameter);

            return await ExecuteAsync<T>(database, queryExp, parameter, cache, cancellationToken: cancellationToken);
        }

        public async Task<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default)
            where T : new()
        {
            query = query.Trim();
            var final = new ArangoList<T>();

            try
            {
                var firstResult = await SendAsync<QueryResponse<T>>(HttpMethod.Post,
                    ApiPath(database, "cursor"),
                    Serialize(new QueryRequest
                    {
                        Query = query,
                        BindVars = bindVars,
                        BatchSize = _context.BatchSize,
                        Cache = cache,
                        Options = new QueryRequestOptions
                        {
                            FullCount = fullCount
                        }
                    }), cancellationToken: cancellationToken);

                final.AddRange(firstResult.Result);

                _context.QueryProfile?.Invoke(query, bindVars, firstResult.Extra.GetValue("stats"));

                if (fullCount.HasValue && fullCount.Value)
                    final.FullCount = firstResult.Extra.GetValue("stats").Value<int>("fullCount");

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
                _context.QueryProfile?.Invoke(query, bindVars, null);
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

            var body = Serialize(new QueryRequest
            {
                Query = query,
                BindVars = bindVars,
                BatchSize = _context.BatchSize,
                Cache = cache,
                Options = new QueryRequestOptions
                {
                    FullCount = fullCount
                }
            });

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
    }
}