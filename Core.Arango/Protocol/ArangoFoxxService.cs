using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoFoxxService
    {
        [JsonPropertyName("mount")]
        [JsonProperty(PropertyName = "mount")]
        public string Mount { get; set; }

        [JsonPropertyName("development")]
        [JsonProperty(PropertyName = "development")]
        public bool Development { get; set; }

        [JsonPropertyName("legacy")]
        [JsonProperty(PropertyName = "legacy")]
        public bool Legacy { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonPropertyName("version")]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}