using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango collection
    /// </summary>
    public class ArangoCollection : ArangoCollectionUpdate
    {
        /// <summary>
        ///   The name of the collection.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///   The type of the collection to create.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoCollectionType? Type { get; set; }
        
        // MMFiles
        /*[JsonPropertyName("doCompact")]
        [JsonProperty(PropertyName = "doCompact", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DoCompact { get; set; }*/

        /// <summary>
        ///  If true, create a system collection.
        ///  In this case collection-name should start with an underscore.
        ///  End users should normally create non-system collections only. API implementors may be required to create system collections in very special occasions, but normally a regular collection will do. (The default is false)
        /// </summary>
        [JsonPropertyName("isSystem")]
        [JsonProperty(PropertyName = "isSystem", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsSystem { get; set; }

        // MMFiles
        /*[JsonPropertyName("isVolatile")]
        [JsonProperty(PropertyName = "isVolatile", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsVolatile { get; set; }*/

        /// <summary>
        ///   (The default is 1): in a cluster, this value determines the number of shards to create for the collection.
        ///   In a single server setup, this option is meaningless.
        /// </summary>
        [JsonPropertyName("numberOfShards")]
        [JsonProperty(PropertyName = "numberOfShards", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NumberOfShards { get; set; }

        // Obsolete?
        /*[JsonPropertyName("indexBuckets")]
        [JsonProperty(PropertyName = "indexBuckets", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? IndexBuckets { get; set; }*/

        /// <summary>
        ///   Additional options for key generation.
        /// </summary>
        [JsonPropertyName("keyOptions")]
        [JsonProperty(PropertyName = "keyOptions", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoKeyOptions KeyOptions { get; set; }

        /// <summary>
        ///   (The default is [ “_key” ]): in a cluster, this attribute determines which document attributes are used to determine the target shard for documents.
        ///   Documents are sent to shards based on the values of their shard key attributes.
        ///   The values of all shard key attributes in a document are hashed, and the hash value is used to determine the target shard. Note: Values of shard key attributes cannot be changed once set.
        ///   This option is meaningless in a single server setup.
        /// </summary>
        [JsonPropertyName("shardKeys")]
        [JsonProperty(PropertyName = "shardKeys", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> ShardKeys { get; set; }

        /// <summary>
        ///   (The default is 1): in a cluster, this attribute determines how many copies of each shard are kept on different DB-Servers.
        ///   The value 1 means that only one copy (no synchronous replication) is kept.
        ///   A value of k means that k-1 replicas are kept.
        ///   It can also be the string "satellite" for a SatelliteCollection, where the replication factor is matched to the number of DB-Servers (Enterprise Edition only).
        /// </summary>
        [JsonPropertyName("replicationFactor")]
        [JsonProperty(PropertyName = "replicationFactor", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object ReplicationFactor { get; set; }

        /// <summary>
        ///   This attribute specifies the name of the sharding strategy to use for the collection.
        ///   Since ArangoDB 3.4 there are different sharding strategies to select from when creating a new collection. The selected shardingStrategy value will remain fixed for the collection and cannot be changed afterwards.
        ///   This is important to make the collection keep its sharding settings and always find documents already distributed to shards using the same initial sharding algorithm.
        ///   community-compat, enterprise-compat, enterprise-smart-edge-compat, hash, enterprise-hash-smart-edge
        /// </summary>
        [JsonPropertyName("shardingStrategy")]
        [JsonProperty(PropertyName = "shardingStrategy", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ShardingStrategy { get; set; }

        /// <summary>
        ///   (The default is ”“): in an Enterprise Edition cluster, this attribute binds the specifics of sharding for the newly created collection to follow that of a specified existing collection.
        ///   Note: Using this parameter has consequences for the prototype collection.
        ///   It can no longer be dropped, before the sharding-imitating collections are dropped.
        ///   Equally, backups and restores of imitating collections alone will generate warnings (which can be overridden) about missing sharding prototype.
        /// </summary>
        [JsonPropertyName("distributeShardsLike")]
        [JsonProperty(PropertyName = "distributeShardsLike", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DistributeShardsLike { get; set; }

        /// <summary>
        ///   Write concern for this collection (default: 1).
        ///   It determines how many copies of each shard are required to be in sync on the different DB-Servers.
        ///   If there are less then these many copies in the cluster a shard will refuse to write.
        ///   Writes to shards with enough up-to-date copies will succeed at the same time however.
        ///   The value of writeConcern can not be larger than replicationFactor. (cluster only)
        /// </summary>
        [JsonPropertyName("writeConcern")]
        [JsonProperty(PropertyName = "writeConcern", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WriteConcern { get; set; }
        
        /// <summary>
        ///   In an Enterprise Edition cluster, this attribute determines an attribute of the collection that must contain the shard key value of the referred-to SmartJoin collection.
        ///   Additionally, the shard key for a document in this collection must contain the value of this attribute, followed by a colon, followed by the actual primary key of the document.
        /// </summary>
        [JsonPropertyName("smartJoinAttribute")]
        [JsonProperty(PropertyName = "smartJoinAttribute", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string SmartJoinAttribute { get; set; }
    }
}