using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoQueryStatistic
    {
        [JsonPropertyName("scannedFull")]
        [JsonProperty(PropertyName = "scannedFull", NullValueHandling = NullValueHandling.Ignore)] 
        public long? ScannedFull { get; set; }

        [JsonPropertyName("scannedIndex")]
        [JsonProperty(PropertyName = "scannedIndex", NullValueHandling = NullValueHandling.Ignore)] 
        public long? ScannedIndex { get; set; }

        [JsonPropertyName("writesExecuted")]
        [JsonProperty(PropertyName = "writesExecuted", NullValueHandling = NullValueHandling.Ignore)] 
        public long? WritesExecuted { get; set; }

        [JsonPropertyName("peakMemoryUsage")]
        [JsonProperty(PropertyName = "peakMemoryUsage", NullValueHandling = NullValueHandling.Ignore)] 
        public long? PeakMemoryUsage { get; set; }

        [JsonPropertyName("executionTime")]
        [JsonProperty(PropertyName = "executionTime", NullValueHandling = NullValueHandling.Ignore)] 
        public double? ExecutionTime { get; set; }

        [JsonPropertyName("fullCount")]
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)] 
        public long? FullCount { get; set; }

        [JsonPropertyName("writesIgnored")]
        [JsonProperty(PropertyName = "writesIgnored", NullValueHandling = NullValueHandling.Ignore)] 
        public long? WritesIgnored { get; set; }

        [JsonPropertyName("filtered")]
        [JsonProperty(PropertyName = "filtered", NullValueHandling = NullValueHandling.Ignore)] 
        public long? Filtered { get; set; }
    }
}