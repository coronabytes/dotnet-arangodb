using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    public interface IArangoQueryModule
    {
        Task<List<T>> FindAsync<T>(ArangoHandle database, string collection, FormattableString filter, string projection = null, int limit = 1000, CancellationToken cancellationToken = default) where T : new();
        Task<object> QueryAsync(Type type, bool isEnumerable, ArangoHandle database, string query, IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default);
        Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, FormattableString query, bool? cache = null, CancellationToken cancellationToken = default) where T : new();
        Task<ArangoList<T>> QueryAsync<T>(ArangoHandle database, string query, IDictionary<string, object> bindVars, bool? cache = null, bool? fullCount = null, CancellationToken cancellationToken = default) where T : new();
        Task<T> SingleOrDefaultAsync<T>(ArangoHandle database, string collection, FormattableString filter, string projection = null, CancellationToken cancellationToken = default) where T : new();
    }
}