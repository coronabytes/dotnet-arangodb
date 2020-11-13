using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoCollection : ArangoCollectionUpdate
    {
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoCollectionType? Type { get; set; }

        [JsonPropertyName("doCompact")]
        [JsonProperty(PropertyName = "doCompact", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? DoCompact { get; set; }

        [JsonPropertyName("isSystem")]
        [JsonProperty(PropertyName = "isSystem", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsSystem { get; set; }

        [JsonPropertyName("isVolatile")]
        [JsonProperty(PropertyName = "isVolatile", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsVolatile { get; set; }

        [JsonPropertyName("numberOfShards")]
        [JsonProperty(PropertyName = "numberOfShards", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NumberOfShards { get; set; }

        [JsonPropertyName("indexBuckets")]
        [JsonProperty(PropertyName = "indexBuckets", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? IndexBuckets { get; set; }

        [JsonPropertyName("keyOptions")]
        [JsonProperty(PropertyName = "keyOptions", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoKeyOptions KeyOptions { get; set; }

        [JsonPropertyName("shardKeys")]
        [JsonProperty(PropertyName = "shardKeys", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> ShardKeys { get; set; }

        [JsonPropertyName("replicationFactor")]
        [JsonProperty(PropertyName = "replicationFactor", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? ReplicationFactor { get; set; }

        [JsonPropertyName("shardingStrategy")]
        [JsonProperty(PropertyName = "shardingStrategy", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ShardingStrategy { get; set; }

        [JsonPropertyName("distributeShardsLike")]
        [JsonProperty(PropertyName = "distributeShardsLike", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string DistributeShardsLike { get; set; }

        [JsonPropertyName("writeConcern")]
        [JsonProperty(PropertyName = "writeConcern", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? WriteConcern { get; set; }
    }
}