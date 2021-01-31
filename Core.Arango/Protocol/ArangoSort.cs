using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoSort
    {
        [JsonPropertyName("field")]
        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonPropertyName("direction")]
        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
    }
}