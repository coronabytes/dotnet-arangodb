using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango Vertex Collection
    /// </summary>
    public class ArangoVertexCollection
    {
        /// <summary>
        ///     Collection name
        /// </summary>
        [JsonPropertyName("collection")]
        [JsonProperty(PropertyName = "collection")]
        public string Collection { get; set; }
    }
}