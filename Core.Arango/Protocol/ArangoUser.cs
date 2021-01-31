using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoUser
    {
        [JsonPropertyName("user")]
        [JsonProperty(PropertyName = "user")]
        public string Name { get; set; }

        [JsonPropertyName("passwd")]
        [JsonProperty(PropertyName = "passwd")]
        public string Password { get; set; }

        [JsonPropertyName("active")]
        [JsonProperty(PropertyName = "active", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

        [JsonPropertyName("extra")]
        [JsonProperty(PropertyName = "extra", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Extra { get; set; }
    }
}