using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   
    /// </summary>
    public class ArangoAstNode
    {
        /// <summary>
        ///   
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        ///   
        /// </summary>
        [JsonPropertyName("subNodes")]
        [JsonProperty(PropertyName = "subNodes")]
        public ICollection<ArangoAstNode> SubNodes { get; set; }

        /// <summary>
        ///   
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///   
        /// </summary>
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }

        /// <summary>
        ///   
        /// </summary>
        [JsonPropertyName("value")]
        [JsonProperty(PropertyName = "value")]
        public object Value { get; set; }
    }
}