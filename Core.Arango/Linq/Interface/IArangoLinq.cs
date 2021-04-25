using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Arango.Linq.Interface
{
    public interface IArangoLinq
    {
        public Func<string, string> TranslateGroupByIntoName { get; }
        public string ResolvePropertyName(Type t, string s);
        public string ResolveCollectionName(Type t);
        public IAsyncEnumerable<T> ExecAsyncEnum<T>(string query, IDictionary<string, object> bindVars);
        public Task<ArangoList<T>> ExecAsync<T>(string query, IDictionary<string, object> bindVars);
    }
}