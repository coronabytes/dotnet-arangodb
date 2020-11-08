using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoVoid
    {

    }

    public class ArangoView
    {
        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; set; }

        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "type")] 
        public string Type { get; set; } = "arangosearch";

        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "links")] 
        public IDictionary<string, ArangoLinkProperty> Links { get; set; }

        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "primarySort", NullValueHandling = NullValueHandling.Ignore)]
        public IList<ArangoSort> PrimarySort { get; set; }

        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "cleanupIntervalStep", NullValueHandling = NullValueHandling.Ignore)]
        public int? CleanupIntervalStep { get; set; }

        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "commitIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        public int? CommitIntervalMsec { get; set; }

        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "consolidationIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        public int? ConsolidationIntervalMsec { get; set; }
    }
}