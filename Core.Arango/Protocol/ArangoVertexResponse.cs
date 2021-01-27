using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoVertexResponse<T> : ArangoResponseBase
    {
        [JsonPropertyName("vertex")]
        [JsonProperty(PropertyName = "vertex")]
        public T Vertex { get; set; }
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