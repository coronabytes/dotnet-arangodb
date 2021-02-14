using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Transport
{
    public interface IArangoTransport
    {
        Task<object> SendAsync(Type type, HttpMethod m, string url, object body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);

        Task<T> SendAsync<T>(HttpMethod m, string url, object body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);

        Task<HttpContent> SendContentAsync(HttpMethod m, string url, HttpContent body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);

        Task<T> WriteBatchAsync<T>(ArangoHandle handle, HttpMethod m, string url, object body = null);
    }
}