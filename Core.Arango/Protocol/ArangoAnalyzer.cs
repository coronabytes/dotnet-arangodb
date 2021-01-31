using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoAnalyzer
    {
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        [JsonProperty(PropertyName = "properties")]
        public ArangoAnalyzerProperties Properties { get; set; }

        [JsonPropertyName("features")]
        [JsonProperty(PropertyName = "features")]
        public IList<string> Features { get; set; }
    }
}