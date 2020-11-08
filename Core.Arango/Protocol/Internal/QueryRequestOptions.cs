using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryRequestOptions
    {
        [JsonPropertyName("fullCount")]
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FullCount { get; set; }

        [JsonPropertyName("optimizer")]
        [JsonProperty(PropertyName = "optimizer", NullValueHandling = NullValueHandling.Ignore)]
        public QueryOptimizer Optimizer { get; set; }
    }
}