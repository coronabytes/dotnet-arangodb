using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryRequestOptions
    {
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FullCount { get; set; }

        [JsonProperty(PropertyName = "optimizer", NullValueHandling = NullValueHandling.Ignore)]
        public QueryOptimizer Optimizer { get; set; }
    }
}