using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Linq.Interface
{
    internal interface IArangoLinq
    {
        public Func<string, string> TranslateGroupByIntoName { get; }
        public string ResolvePropertyName(Type t, string s);
        public string ResolveCollectionName(Type t);
        public IAsyncEnumerable<T> StreamAsync<T>(string query, IDictionary<string, object> bindVars, CancellationToken cancellationToken = default);
        public Task<ArangoList<T>> ExecuteAsync<T>(string query, IDictionary<string, object> bindVars, CancellationToken cancellationToken = default);
    }
}