using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///    Arango Query Statistic
    /// </summary>
    public class ArangoQueryStatistic
    {
        /// <summary>
        ///   Documents processed with a full collection scan
        /// </summary>
        [JsonPropertyName("scannedFull")]
        [JsonProperty(PropertyName = "scannedFull", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ScannedFull { get; set; }

        /// <summary>
        ///   Documents processed with a index scan
        /// </summary>
        [JsonPropertyName("scannedIndex")]
        [JsonProperty(PropertyName = "scannedIndex", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? ScannedIndex { get; set; }

        /// <summary>
        ///   Documents which have been written
        /// </summary>
        [JsonPropertyName("writesExecuted")]
        [JsonProperty(PropertyName = "writesExecuted", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? WritesExecuted { get; set; }

        /// <summary>
        ///   Memory usage in bytes
        /// </summary>
        [JsonPropertyName("peakMemoryUsage")]
        [JsonProperty(PropertyName = "peakMemoryUsage", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? PeakMemoryUsage { get; set; }

        /// <summary>
        ///   Execution time in ms
        /// </summary>
        [JsonPropertyName("executionTime")]
        [JsonProperty(PropertyName = "executionTime", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? ExecutionTime { get; set; }

        /// <summary>
        ///  Result size ignoring LIMIT clause
        /// </summary>
        [JsonPropertyName("fullCount")]
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? FullCount { get; set; }

        /// <summary>
        ///  Document writes ignored
        /// </summary>
        [JsonPropertyName("writesIgnored")]
        [JsonProperty(PropertyName = "writesIgnored", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? WritesIgnored { get; set; }

        /// <summary>
        ///  Filtered documents
        /// </summary>
        [JsonPropertyName("filtered")]
        [JsonProperty(PropertyName = "filtered", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Filtered { get; set; }
    }
}