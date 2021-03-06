using System.Threading.Tasks;

namespace Core.Arango.Modules
{
    /// <summary>
    ///  Batches multiple operations in single HTTP request (round trip optimization)
    /// </summary>
    public interface IArangoBatchModule
    {
        /// <summary>
        ///  Create batch handle from database handle
        /// </summary>
        public ArangoHandle Create(ArangoHandle handle);

        /// <summary>
        ///  Execute batch against Arango server
        /// </summary>
        public Task ExecuteAsync(ArangoHandle handle);
    }
}