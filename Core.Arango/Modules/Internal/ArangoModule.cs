using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Arango.Modules.Internal
{
    internal abstract class ArangoModule
    {
        private readonly IArangoContext _context;

        protected ArangoModule(IArangoContext context)
        {
            _context = context;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, ArangoContext.JsonSerializerSettings);
        }
        public string Realm => _context.Realm;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string RealmPrefix(string name)
        {
            if (name == "_system")
                return "_system";

            return UrlEncoder.Default.Encode(_context.Realm + name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string ApiPath(ArangoHandle handle, string path)
        {
            return $"{_context.Server}/_db/{_context.DbName(handle)}/_api/{path}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string ApiPath(string path)
        {
            return $"{_context.Server}/_api/{path}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected string UrlEncode(string value)
        {
            return UrlEncoder.Default.Encode(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<T> SendAsync<T>(HttpMethod m, string url, string body = null,
            string transaction = null, bool throwOnError = true, bool auth = true,
            CancellationToken cancellationToken = default)
        {
            return _context.SendAsync<T>(m, url, body, transaction, throwOnError, auth, cancellationToken);
        }
    }
}