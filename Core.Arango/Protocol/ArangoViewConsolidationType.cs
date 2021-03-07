using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango View Consolidation Type
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoViewConsolidationType
    {
        /// <summary>
        ///   (default): consolidate based on segment byte size and live document count as dictated by the customization attributes.
        ///   If this type is used, then below segments* and minScore properties are available.
        /// </summary>
        [EnumMember(Value = "tier")]
        Tier, 
        /// <summary>
        ///   Consolidate if and only if {threshold} > (segment_bytes + sum_of_merge_candidate_segment_bytes) / all_segment_bytes i.e. the sum of all candidate segment byte size is less than the total segment byte size multiplied by the {threshold}.
        ///   If this type is used, then below threshold property is available.
        /// </summary>
        [EnumMember(Value = "bytes_accum")]
        BytesAccum
    }
}