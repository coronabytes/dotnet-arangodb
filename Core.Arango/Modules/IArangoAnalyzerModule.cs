using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Arango.Protocol;

namespace Core.Arango.Modules
{
    /// <summary>
    ///     ArangoSearch analyzer
    /// </summary>
    public interface IArangoAnalyzerModule
    {
        /// <summary>
        ///     creates a new Analyzer based on the provided definition
        /// </summary>
        Task CreateAsync(ArangoHandle database, ArangoAnalyzer analyzer, CancellationToken cancellationToken = default);

        /// <summary>
        ///     removes an Analyzer configuration
        /// </summary>
        /// <param name="database"></param>
        /// <param name="analyzer">The name of the Analyzer to remove.</param>
        /// <param name="force">The Analyzer configuration should be removed even if it is in-use. The default value is false.</param>
        /// <param name="cancellationToken"></param>
        Task DeleteAsync(ArangoHandle database, string analyzer, bool force = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     returns a listing of available Analyzer definitions
        /// </summary>
        Task<IReadOnlyCollection<ArangoAnalyzer>> ListAsync(ArangoHandle database, CancellationToken cancellationToken = default);

        /// <summary>
        ///  returns an Analyzer definition
        /// </summary>
        Task<ArangoAnalyzer> GetDefinitionAsync(ArangoHandle database, string analyzer, CancellationToken cancellationToken = default);
    }
}