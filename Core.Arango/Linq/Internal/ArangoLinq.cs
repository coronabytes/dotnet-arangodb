using System;
using System.Collections.Generic;
using System.Threading;
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

        public IAsyncEnumerable<T> StreamAsync<T>(string query, IDictionary<string, object> bindVars, CancellationToken cancellationToken = default)
        {
            return _context.Query.ExecuteStreamAsync<T>(_handle, query, bindVars, cancellationToken: cancellationToken);
        }

        public async ValueTask<ArangoList<T>> ExecuteAsync<T>(string query, IDictionary<string, object> bindVars, CancellationToken cancellationToken = default)
        {
            return await _context.Query.ExecuteAsync<T>(_handle, query, bindVars, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}