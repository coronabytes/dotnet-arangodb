using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoEdgeResponse<T> : ArangoResponseBase
    {
        [JsonPropertyName("edge")]
        [JsonProperty(PropertyName = "edge")]
        public T Edge { get; set; }

        [JsonPropertyName("new")]
        [JsonProperty(PropertyName = "new")]
        public T New { get; set; }

        [JsonPropertyName("old")]
        [JsonProperty(PropertyName = "old")]
        public T Old { get; set; }

        [JsonPropertyName("removed")]
        [JsonProperty(PropertyName = "removed")]
        public bool Removed { get; set; }
    }
}