using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoVertexCollection
    {
        [JsonPropertyName("collection")]
        [JsonProperty(PropertyName = "collection")]
        public string Collection { get; set; }
    }

    public class ArangoEdgeDefinition
    {
        [JsonPropertyName("collection")]
        [JsonProperty(PropertyName = "collection")]
        public string Collection { get; set; }

        [JsonPropertyName("from")]
        [JsonProperty(PropertyName = "from")]
        public IList<string> From { get; set; }

        [JsonPropertyName("to")]
        [JsonProperty(PropertyName = "to")]
        public IList<string> To { get; set; }
    }
}