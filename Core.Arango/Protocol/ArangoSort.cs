using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     ArangoView Sort description
    /// </summary>
    public class ArangoSort
    {
        /// <summary>
        ///     Attribute to sort by
        /// </summary>
        [JsonPropertyName("field")]
        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        /// <summary>
        ///     Sort direction
        /// </summary>
        [JsonPropertyName("direction")]
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
    }
}