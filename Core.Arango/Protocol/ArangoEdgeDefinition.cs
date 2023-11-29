using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Edge definition
    /// </summary>
    public class ArangoEdgeDefinition
    {
        /// <summary>
        ///     The name of the edge collection to be used.
        /// </summary>
        [JsonPropertyName("collection")]
        [JsonProperty(PropertyName = "collection")]
        public string Collection { get; set; }

        /// <summary>
        ///     One or many vertex collections that can contain source vertices.
        /// </summary>
        [JsonPropertyName("from")]
        [JsonProperty(PropertyName = "from")]
        public List<string> From { get; set; }

        /// <summary>
        ///     One or many vertex collections that can contain target vertices.
        /// </summary>
        [JsonPropertyName("to")]
        [JsonProperty(PropertyName = "to")]
        public List<string> To { get; set; }
    }
}