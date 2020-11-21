using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoQueryStatistic
    {
        [JsonPropertyName("scannedFull")]
        [JsonProperty(PropertyName = "scannedFull", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ScannedFull { get; set; }

        [JsonPropertyName("scannedIndex")]
        [JsonProperty(PropertyName = "scannedIndex", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ScannedIndex { get; set; }

        [JsonPropertyName("writesExecuted")]
        [JsonProperty(PropertyName = "writesExecuted", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? WritesExecuted { get; set; }

        [JsonPropertyName("peakMemoryUsage")]
        [JsonProperty(PropertyName = "peakMemoryUsage", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? PeakMemoryUsage { get; set; }

        [JsonPropertyName("executionTime")]
        [JsonProperty(PropertyName = "executionTime", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? ExecutionTime { get; set; }

        [JsonPropertyName("fullCount")]
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? FullCount { get; set; }

        [JsonPropertyName("writesIgnored")]
        [JsonProperty(PropertyName = "writesIgnored", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? WritesIgnored { get; set; }

        [JsonPropertyName("filtered")]
        [JsonProperty(PropertyName = "filtered", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Filtered { get; set; }
    }
}