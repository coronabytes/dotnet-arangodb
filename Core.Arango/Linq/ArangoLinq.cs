using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Linq.Interface;

namespace Core.Arango.Linq
{
    internal class ArangoLinq : IArangoLinq
    {
        private readonly IArangoContext _context;
        private readonly ArangoHandle _handle;

        public ArangoLinq(IArangoContext context, ArangoHandle handle)
        {
            _context = context;
            _handle = handle;
        }

        public string ResolvePropertyName(Type t, string s)
        {
            return _context.Configuration.ResolveProperty(t, s);
        }

        public string ResolveCollectionName(Type t)
        {
            return _context.Configuration.ResolveCollection(t);
        }

        public Func<string, string> TranslateGroupByIntoName => _context.Configuration.ResolveGroupBy;

        public IAsyncEnumerable<T> ExecAsyncEnum<T>(string query, IDictionary<string, object> bindVars)
        {
            return _context.Query.ExecuteStreamAsync<T>(_handle, query, bindVars);
        }

        public async Task<ArangoList<T>> ExecAsync<T>(string query, IDictionary<string, object> bindVars)
        {
            return await _context.Query.ExecuteAsync<T>(_handle, query, bindVars).ConfigureAwait(false);
        }
    }
}