using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango Graph Vertex Response
    /// </summary>
    public class ArangoVertexResponse<T> : ArangoResponseBase
    {
        /// <summary>
        ///  Vertex
        /// </summary>
        [JsonPropertyName("vertex")]
        [JsonProperty(PropertyName = "vertex")]
        public T Vertex { get; set; }

        /// <summary>
        ///  New version
        /// </summary>
        [JsonPropertyName("new")]
        [JsonProperty(PropertyName = "new")]
        public T New { get; set; }

        /// <summary>
        ///  Old version
        /// </summary>
        [JsonPropertyName("old")]
        [JsonProperty(PropertyName = "old")]
        public T Old { get; set; }

        /// <summary>
        ///  When Removed
        /// </summary>
        [JsonPropertyName("removed")]
        [JsonProperty(PropertyName = "removed")]
        public bool Removed { get; set; }
    }
}