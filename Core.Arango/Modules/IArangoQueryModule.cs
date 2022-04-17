using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     Query documents
    /// </summary>
    public interface IArangoQueryModule
    {
        /// <summary>
        ///     Finds documents
        /// </summary>
        /// <typeparam name="T">RETURN element type</typeparam>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="filter">FILTER expression with "x."</param>
        /// <param name="projection">RETURN expression</param>
        /// <param name="limit"></param>
        /// <param name="cancellationToken"></param>
        Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, int limit = 1000, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Finds single document (and throws exception of more than one are found)
        /// </summary>
        /// <typeparam name="T">RETURN element type</typeparam>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        /// <param name="filter">FILTER expression with "x."</param>
        /// <param name="projection">RETURN expression</param>
        /// <param name="cancellationToken"></param>
        Task<T> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter,
            string projection = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Execute query (Linq provider)
        /// </summary>
        Task<object> ExecuteAsync(Type type, bool isEnumerable, ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Execute query with string interpolated bind parameters
        /// </summary>
        Task<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, FormattableString query, bool? cache = null,
            bool? fullCount = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Execute query with bind parameters in dictionary
        /// </summary>
        Task<ArangoList<T>> ExecuteAsync<T>(ArangoHandle database, string query, IDictionary<string, object> bindVars,
            bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///     Execute query with string interpolated bind parameters (IAsyncEnumerable)
        /// </summary>
        IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, FormattableString query, bool? cache = null,
            int? batchSize = null,
            bool? lazy = null, bool? fillBlockCache = null, long? memoryLimit = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Execute query with bind parameters in dictionary (IAsyncEnumerable)
        /// </summary>
        IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, string query,
            IDictionary<string, object> bindVars, bool? cache = null, int? batchSize = null,
            double? ttl = null, long? memoryLimit = null,
            ArangoQueryOptions options = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Execute query (IAsyncEnumerable)
        /// </summary>
        IAsyncEnumerable<T> ExecuteStreamAsync<T>(ArangoHandle database, ArangoCursor cursor,
            CancellationToken cancellationToken = default);


        /// <summary>
        ///     explain an AQL query and return information about it
        /// </summary>
        Task<ArangoExplainResult> ExplainAsync(ArangoHandle database, string query,
            IDictionary<string, object> bindVars,
            bool allPlans = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     explain an AQL query and return information about it
        /// </summary>
        Task<ArangoExplainResult> ExplainAsync(ArangoHandle database, FormattableString query,
            bool allPlans = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     parse an AQL query and return information about it
        /// </summary>
        Task<ArangoParseResult> ParseAsync(ArangoHandle database, string query,
            CancellationToken cancellationToken = default);
    }
}