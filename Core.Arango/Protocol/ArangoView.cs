using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoView
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "type")] public string Type { get; set; } = "arangosearch";

        [JsonProperty(PropertyName = "links")] public IDictionary<string, ArangoLinkProperty> Links { get; set; }

        [JsonProperty(PropertyName = "primarySort", NullValueHandling = NullValueHandling.Ignore)]
        public IList<ArangoSort> PrimarySort { get; set; }

        [JsonProperty(PropertyName = "cleanupIntervalStep", NullValueHandling = NullValueHandling.Ignore)]
        public int? CleanupIntervalStep { get; set; }

        [JsonProperty(PropertyName = "commitIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        public int? CommitIntervalMsec { get; set; }

        [JsonProperty(PropertyName = "consolidationIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        public int? ConsolidationIntervalMsec { get; set; }
    }
}