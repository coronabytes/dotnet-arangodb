using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Arango.Transport
{
    /// <summary>
    ///  Arango Transport Interface
    /// </summary>
    public interface IArangoTransport
    {
        /// <summary>
        ///  Send request to ArangoDB
        /// </summary>
        Task<object> SendAsync(Type type, HttpMethod m, string url, object body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Send request to ArangoDB
        /// </summary>
        Task<T> SendAsync<T>(HttpMethod m, string url, object body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Send raw HTTP content request to ArangoDB
        /// </summary>
        Task<HttpContent> SendContentAsync(HttpMethod m, string url, HttpContent body = null, string transaction = null,
            bool throwOnError = true, bool auth = true, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Send request to batch to be later send to ArangoDB
        /// </summary>
        Task<T> WriteBatchAsync<T>(ArangoHandle handle, HttpMethod m, string url, object body = null);
    }
}