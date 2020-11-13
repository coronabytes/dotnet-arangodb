using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryRequestOptions
    {
        [JsonPropertyName("fullCount")]
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FullCount { get; set; }

        [JsonPropertyName("optimizer")]
        [JsonProperty(PropertyName = "optimizer", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public QueryOptimizer Optimizer { get; set; }
    }
}