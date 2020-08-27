using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Core.Arango.Protocol
{
    public class ArangoCollectionUpdate
    {
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WaitForSync { get; set; }

        [JsonProperty(PropertyName = "journalSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? JournalSize { get; set; }

        [JsonProperty(PropertyName = "schema", NullValueHandling = NullValueHandling.Ignore)]
        public JSchema Schema { get; set; }
    }

    public class ArangoCollection : ArangoCollectionUpdate
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public ArangoCollectionType? Type { get; set; }

        [JsonProperty(PropertyName = "doCompact", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DoCompact { get; set; }

        [JsonProperty(PropertyName = "isSystem", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSystem { get; set; }

        [JsonProperty(PropertyName = "isVolatile", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVolatile { get; set; }

        [JsonProperty(PropertyName = "numberOfShards", NullValueHandling = NullValueHandling.Ignore)]
        public int? NumberOfShards { get; set; }

        [JsonProperty(PropertyName = "indexBuckets", NullValueHandling = NullValueHandling.Ignore)]
        public int? IndexBuckets { get; set; }

        [JsonProperty(PropertyName = "keyOptions", NullValueHandling = NullValueHandling.Ignore)]
        public ArangoKeyOptions KeyOptions { get; set; }

        [JsonProperty(PropertyName = "shardKeys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ShardKeys { get; set; }

        [JsonProperty(PropertyName = "replicationFactor", NullValueHandling = NullValueHandling.Ignore)]
        public int? ReplicationFactor { get; set; }

        [JsonProperty(PropertyName = "shardingStrategy", NullValueHandling = NullValueHandling.Ignore)]
        public string ShardingStrategy { get; set; }

        [JsonProperty(PropertyName = "distributeShardsLike", NullValueHandling = NullValueHandling.Ignore)]
        public string DistributeShardsLike { get; set; }

        [JsonProperty(PropertyName = "writeConcern", NullValueHandling = NullValueHandling.Ignore)]
        public int? WriteConcern { get; set; }
    }
}