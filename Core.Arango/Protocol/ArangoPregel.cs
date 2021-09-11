using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Pregel Job
    /// </summary>
    public class ArangoPregel
    {
        /// <summary>
        ///     Name of the algorithm
        /// </summary>
        [JsonPropertyName("algorithm")]
        [JsonProperty(PropertyName = "algorithm")]
        public string Algorithm { get; set; }

        /// <summary>
        ///     Name of a graph. Either this or the parameters vertexCollections and edgeCollections are required. Please note that
        ///     there are special sharding requirements for graphs in order to be used with Pregel.
        /// </summary>
        [JsonPropertyName("graphName")]
        [JsonProperty(PropertyName = "graphName")]
        public string GraphName { get; set; }

        /// <summary>
        ///     List of vertex collection names. Please note that there are special sharding requirements for collections in order
        ///     to be used with Pregel.
        /// </summary>
        [JsonPropertyName("vertexCollections")]
        [JsonProperty(PropertyName = "vertexCollections")]
        public ICollection<string> VertexCollections { get; set; }

        /// <summary>
        ///     List of edge collection names. Please note that there are special sharding requirements for collections in order to
        ///     be used with Pregel.
        /// </summary>
        [JsonPropertyName("edgeCollections")]
        [JsonProperty(PropertyName = "edgeCollections")]
        public ICollection<string> EdgeCollections { get; set; }

        /// <summary>
        ///     General as well as algorithm-specific options.
        /// </summary>
        [JsonPropertyName("params")]
        [JsonProperty(PropertyName = "params")]
        public IDictionary<string, object> Params { get; set; }
    }
}