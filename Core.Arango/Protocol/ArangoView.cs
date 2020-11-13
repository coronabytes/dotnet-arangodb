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

        [JsonPropertyName("primarySort")]
        [JsonProperty(PropertyName = "primarySort", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<ArangoSort> PrimarySort { get; set; }

        [JsonPropertyName("cleanupIntervalStep")]
        [JsonProperty(PropertyName = "cleanupIntervalStep", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CleanupIntervalStep { get; set; }

        [JsonPropertyName("commitIntervalMsec")]
        [JsonProperty(PropertyName = "commitIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CommitIntervalMsec { get; set; }

        [JsonPropertyName("consolidationIntervalMsec")]
        [JsonProperty(PropertyName = "consolidationIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ConsolidationIntervalMsec { get; set; }
    }
}