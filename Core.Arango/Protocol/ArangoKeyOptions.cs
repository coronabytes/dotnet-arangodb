using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoKeyOptions
    {
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoKeyType? Type { get; set; }

        [JsonPropertyName("allowUserKeys")]
        [JsonProperty(PropertyName = "allowUserKeys", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowUserKeys { get; set; }

        [JsonPropertyName("increment")]
        [JsonProperty(PropertyName = "increment", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Increment { get; set; }

        [JsonPropertyName("offset")]
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Offset { get; set; }
    }
}