using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoKeyOptions
    {
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public ArangoKeyType? Type { get; set; }

        [JsonPropertyName("allowUserKeys")]
        [JsonProperty(PropertyName = "allowUserKeys", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowUserKeys { get; set; }

        [JsonPropertyName("increment")]
        [JsonProperty(PropertyName = "increment", NullValueHandling = NullValueHandling.Ignore)]
        public long? Increment { get; set; }

        [JsonPropertyName("offset")]
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        public long? Offset { get; set; }
    }
}