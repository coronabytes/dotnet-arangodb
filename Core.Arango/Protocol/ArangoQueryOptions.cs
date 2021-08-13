using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango Query Options
    /// </summary>
    public class ArangoQueryOptions
    {
        /// <summary>
        /// if set to true and the query contains a LIMIT clause, then the result will have an extra attribute with the sub-attributes stats and fullCount
        /// </summary>
        [JsonPropertyName("fullCount")]
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FullCount { get; set; }

        /// <summary>
        ///   if set to true or not specified, this will make the query store the data it reads via the RocksDB storage engine in the RocksDB block cache
        /// </summary>
        [JsonPropertyName("fillBlockCache")]
        [JsonProperty(PropertyName = "fillBlockCache", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FillBlockCache { get; set; }

        /// <summary>
        ///  Limits the maximum number of plans that are created by the AQL query optimizer.
        /// </summary>
        [JsonPropertyName("maxPlans")]
        [JsonProperty(PropertyName = "maxPlans", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxPlans { get; set; }

        /// <summary>
        ///   Limits the maximum number of warnings a query will return. The number of warnings a query will return is limited to 10 by default, but that number can be increased or decreased by setting this attribute.
        /// </summary>
        [JsonPropertyName("maxWarningCount")]
        [JsonProperty(PropertyName = "maxWarningCount", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxWarningCount { get; set; }

        /// <summary>
        /// When set to true, the query will throw an exception and abort instead of producing a warning
        /// </summary>
        [JsonPropertyName("failOnWarning")]
        [JsonProperty(PropertyName = "failOnWarning", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FailOnWarning { get; set; }

        /// <summary>
        /// Can be enabled to execute the query lazily. If set to true, then the query is executed as long as necessary to produce up to batchSize results. These results are returned immediately and the query is suspended until the client asks for the next batch (if there are more results).
        /// </summary>
        [JsonPropertyName("stream")]
        [JsonProperty(PropertyName = "stream", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Stream { get; set; }

        /// <summary>
        /// Options related to the query optimizer.
        /// </summary>
        [JsonPropertyName("optimizer")]
        [JsonProperty(PropertyName = "optimizer", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoQueryOptimizer Optimizer { get; set; }
    }
}