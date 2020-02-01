using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class KeyOptions
    {
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "allowUserKeys", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowUserKeys { get; set; }

        [JsonProperty(PropertyName = "increment", NullValueHandling = NullValueHandling.Ignore)]
        public long? Increment { get; set; }

        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        public long? Offset { get; set; }
    }
}