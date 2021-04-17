using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango View Consolidation Policy
    /// </summary>
    public class ArangoViewConsolidationPolicy
    {
        /// <summary>
        ///     The segment candidates for the “consolidation” operation are selected based upon several possible configurable
        ///     formulas as defined by their types.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public ArangoViewConsolidationType Type { get; set; }

        /// <summary>
        ///     value in the range [0.0, 1.0]
        /// </summary>
        [JsonPropertyName("threshold")]
        [JsonProperty(PropertyName = "threshold", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? Threshold { get; set; }

        /// <summary>
        ///     Defines the value (in bytes) to treat all smaller segments as equal for consolidation selection (default: 2097152)
        /// </summary>
        [JsonPropertyName("segmentsBytesFloor")]
        [JsonProperty(PropertyName = "segmentsBytesFloor", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? SegmentsBytesFloor { get; set; }

        /// <summary>
        ///     Maximum allowed size of all consolidated segments in bytes (default: 5368709120)
        /// </summary>
        [JsonPropertyName("segmentsBytesMax")]
        [JsonProperty(PropertyName = "segmentsBytesMax",  NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? SegmentsBytesMax { get; set; }

        /// <summary>
        ///     The maximum number of segments that will be evaluated as candidates for consolidation (default: 10)
        /// </summary>
        [JsonPropertyName("segmentsMax")]
        [JsonProperty(PropertyName = "segmentsMax",  NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? SegmentsMax { get; set; }

        /// <summary>
        ///     The minimum number of segments that will be evaluated as candidates for consolidation (default: 1)
        /// </summary>
        [JsonPropertyName("segmentsMin")]
        [JsonProperty(PropertyName = "segmentsMin",  NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? SegmentsMin { get; set; }

        /// <summary>
        ///     ?
        /// </summary>
        [JsonPropertyName("minScore")]
        [JsonProperty(PropertyName = "minScore", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? MinScore { get; set; }

        /// <summary>
        ///  Overflow properties
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}