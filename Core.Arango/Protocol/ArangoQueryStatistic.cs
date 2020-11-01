using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoQueryStatistic
    {
        [JsonProperty(PropertyName = "scannedFull", NullValueHandling = NullValueHandling.Ignore)] 
        public long? ScannedFull { get; set; }

        [JsonProperty(PropertyName = "scannedIndex", NullValueHandling = NullValueHandling.Ignore)] 
        public long? ScannedIndex { get; set; }

        [JsonProperty(PropertyName = "writesExecuted", NullValueHandling = NullValueHandling.Ignore)] 
        public long? WritesExecuted { get; set; }

        [JsonProperty(PropertyName = "peakMemoryUsage", NullValueHandling = NullValueHandling.Ignore)] 
        public long? PeakMemoryUsage { get; set; }

        [JsonProperty(PropertyName = "executionTime", NullValueHandling = NullValueHandling.Ignore)] 
        public double? ExecutionTime { get; set; }

        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)] 
        public long? FullCount { get; set; }

        [JsonProperty(PropertyName = "writesIgnored", NullValueHandling = NullValueHandling.Ignore)] 
        public long? WritesIgnored { get; set; }

        [JsonProperty(PropertyName = "filtered", NullValueHandling = NullValueHandling.Ignore)] 
        public long? Filtered { get; set; }
    }
}