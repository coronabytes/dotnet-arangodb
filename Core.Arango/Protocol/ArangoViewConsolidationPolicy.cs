using System.Collections.Generic;

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
        public ArangoViewConsolidationType Type { get; set; }

        /// <summary>
        ///     value in the range [0.0, 1.0]
        /// </summary>
        public double? Threshold { get; set; }

        /// <summary>
        ///     Defines the value (in bytes) to treat all smaller segments as equal for consolidation selection (default: 2097152)
        /// </summary>
        public long? SegmentsBytesFloor { get; set; }

        /// <summary>
        ///     Maximum allowed size of all consolidated segments in bytes (default: 5368709120)
        /// </summary>
        public long? SegmentsBytesMax { get; set; }

        /// <summary>
        ///     The maximum number of segments that will be evaluated as candidates for consolidation (default: 10)
        /// </summary>
        public long? SegmentsMax { get; set; }

        /// <summary>
        ///     The minimum number of segments that will be evaluated as candidates for consolidation (default: 1)
        /// </summary>
        public long? SegmentsMin { get; set; }

        /// <summary>
        ///     ?
        /// </summary>
        public long? MinScore { get; set; }

        /// <summary>
        ///  Overflow properties
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}