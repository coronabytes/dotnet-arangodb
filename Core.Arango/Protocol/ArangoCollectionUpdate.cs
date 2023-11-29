using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango collections updatable properties
    /// </summary>
    public class ArangoCollectionUpdate
    {
        /// <summary>
        ///     If true then the data is synchronized to disk before returning from a document create, update, replace or removal
        ///     operation. (default: false)
        /// </summary>
        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WaitForSync { get; set; }

        // MMFiles
        /*[JsonPropertyName("journalSize")]
        [JsonProperty(PropertyName = "journalSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? JournalSize { get; set; }*/

        /// <summary>
        ///     Optional object that specifies the collection level schema for documents.
        ///     When updating collection this needs to contain existing schema or schema will be removed
        /// </summary>
        [JsonPropertyName("schema")]
        [JsonProperty(PropertyName = "schema", NullValueHandling = NullValueHandling.Include)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ArangoSchema Schema { get; set; }

        /// <summary>
        ///     Whether the in-memory hash cache for documents should be enabled for this collection (default: false).
        /// </summary>
        [JsonPropertyName("cacheEnabled")]
        [JsonProperty(PropertyName = "cacheEnabled", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CacheEnabled { get; set; }

        /// <summary>
        ///     (The default is 1): in a cluster, this attribute determines how many copies of each shard are kept on different
        ///     DB-Servers.
        ///     The value 1 means that only one copy (no synchronous replication) is kept.
        ///     A value of k means that k-1 replicas are kept.
        ///     It can also be the string "satellite" for a SatelliteCollection, where the replication factor is matched to the
        ///     number of DB-Servers (Enterprise Edition only).
        /// </summary>
        [JsonPropertyName("replicationFactor")]
        [JsonProperty(PropertyName = "replicationFactor", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object ReplicationFactor { get; set; }

        /// <summary>
        ///     Write concern for this collection (default: 1).
        ///     It determines how many copies of each shard are required to be in sync on the different DB-Servers.
        ///     If there are less then these many copies in the cluster a shard will refuse to write.
        ///     Writes to shards with enough up-to-date copies will succeed at the same time however.
        ///     The value of writeConcern can not be larger than replicationFactor. (cluster only)
        /// </summary>
        [JsonPropertyName("writeConcern")]
        [JsonProperty(PropertyName = "writeConcern", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WriteConcern { get; set; }

        /// <summary>
        ///     An optional list of objects, each representing a computed value.
        /// </summary>
        [JsonPropertyName("computedValues")]
        [JsonProperty(PropertyName = "computedValues", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ArangoComputedValue> ComputedValues { get; set; }
    }
}