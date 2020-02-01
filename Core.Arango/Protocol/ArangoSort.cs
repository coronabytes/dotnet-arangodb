using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoSort
    {
        [JsonProperty(PropertyName = "field")] public string Field { get; set; }

        [JsonProperty(PropertyName = "direction")]
        public string Direction { get; set; }
    }
}