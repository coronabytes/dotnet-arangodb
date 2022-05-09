using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public abstract class ArangoViewMutation
    {
        /// <summary>
        ///     The name of the View.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     Expects an object with the attribute keys being names of to be linked collections, and the link properties as
        ///     attribute values.
        /// </summary>
        [JsonPropertyName("links")]
        [JsonProperty(PropertyName = "links")]
        public Dictionary<string, ArangoLinkProperty> Links { get; set; }

        /// <summary>
        ///     Wait at least this many commits between removing unused files in the ArangoSearch data directory (default: 2, to
        ///     disable use: 0).
        /// </summary>
        [JsonPropertyName("cleanupIntervalStep")]
        [JsonProperty(PropertyName = "cleanupIntervalStep", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CleanupIntervalStep { get; set; }

        /// <summary>
        ///     Wait at least this many milliseconds between committing View data store changes and making documents visible to
        ///     queries (default: 1000, to disable use: 0)
        /// </summary>
        [JsonPropertyName("commitIntervalMsec")]
        [JsonProperty(PropertyName = "commitIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? CommitIntervalMsec { get; set; }

        /// <summary>
        ///     The consolidation policy to apply for selecting which segments should be merged
        /// </summary>
        [JsonPropertyName("consolidationPolicy")]
        [JsonProperty(PropertyName = "consolidationPolicy", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoViewConsolidationPolicy ConsolidationPolicy { get; set; }

        /// <summary>
        ///     Wait at least this many milliseconds between applying ‘consolidationPolicy’ to consolidate View data store and
        ///     possibly release space on the filesystem (default: 10000, to disable use: 0).
        /// </summary>
        [JsonPropertyName("consolidationIntervalMsec")]
        [JsonProperty(PropertyName = "consolidationIntervalMsec", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ConsolidationIntervalMsec { get; set; }
    }

    public class ArangoViewUpdate : ArangoViewMutation
    {

    }

    public class ArangoViewPatch : ArangoViewMutation
    {

    }

    /// <summary>
    ///     ArangoSearch View
    /// </summary>
    public class ArangoView : ArangoViewMutation
    {

        /// <summary>
        ///     The type of the View. Must be equal to “arangosearch”. This option is immutable.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = "arangosearch";

        /// <summary>
        ///     A primary sort order can be defined to enable an AQL optimization.
        ///     If a query iterates over all documents of a View, wants to sort them by attribute values and the (left-most) fields
        ///     to sort by as well as their sorting direction match with the primarySort definition, then the SORT operation is
        ///     optimized away.
        ///     This option is immutable.
        /// </summary>
        [JsonPropertyName("primarySort")]
        [JsonProperty(PropertyName = "primarySort", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<ArangoSort> PrimarySort { get; set; }

        /// <summary>
        ///     Defines how to compress the primary sort data (introduced in v3.7.1). ArangoDB v3.5 and v3.6 always compress the
        ///     index using LZ4.
        ///     This option is immutable.
        /// </summary>
        [JsonPropertyName("primarySortCompression")]
        [JsonProperty(PropertyName = "primarySortCompression", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoViewCompressionType? PrimarySortCompression { get; set; }

        /// <summary>
        ///     An array of objects to describe which document attributes to store in the View index (introduced in v3.7.1).
        ///     It can then cover search queries, which means the data can be taken from the index directly and accessing the
        ///     storage engine can be avoided.
        /// </summary>
        [JsonPropertyName("storedValues")]
        [JsonProperty(PropertyName = "storedValues", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<ArangoViewStoredValue> StoredValues { get; set; }

        /// <summary>
        ///     Maximum number of writers (segments) cached in the pool (default: 64, use 0 to disable, immutable)
        /// </summary>
        [JsonPropertyName("writebufferIdle")]
        [JsonProperty(PropertyName = "writebufferIdle", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WritebufferIdle { get; set; }

        /// <summary>
        ///     Maximum number of concurrent active writers (segments) that perform a transaction. Other writers (segments) wait
        ///     till current active writers (segments) finish (default: 0, use 0 to disable, immutable)
        /// </summary>
        [JsonPropertyName("writebufferActive")]
        [JsonProperty(PropertyName = "writebufferActive", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WritebufferActive { get; set; }

        /// <summary>
        ///     Maximum memory byte size per writer (segment) before a writer (segment) flush is triggered.
        ///     0 value turns off this limit for any writer (buffer) and data will be flushed periodically based on the value
        ///     defined for the flush thread (ArangoDB server startup option).
        ///     0 value should be used carefully due to high potential memory consumption (default: 33554432, use 0 to disable,
        ///     immutable)
        /// </summary>
        [JsonPropertyName("writebufferSizeMax")]
        [JsonProperty(PropertyName = "writebufferSizeMax", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WritebufferSizeMax { get; set; }

        /// <summary>
        ///     Overflow properties
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}