using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoSchema
    {
        [JsonPropertyName("rule")]
        [JsonProperty(PropertyName = "rule")]
        public object Rule { get; set; }

        [JsonPropertyName("level")]
        [JsonProperty(PropertyName = "level")]
        public ArangoSchemaLevel Level { get; set; } = ArangoSchemaLevel.Strict;

        [JsonPropertyName("message")]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}