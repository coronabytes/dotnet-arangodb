using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    internal class CollectionCreateRequest
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WaitForSync { get; set; }

        [JsonProperty(PropertyName = "journalSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? JournalSize { get; set; }

        [JsonProperty(PropertyName = "doCompact", NullValueHandling = NullValueHandling.Ignore)]
        public bool? DoCompact { get; set; }

        [JsonProperty(PropertyName = "isSystem", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSystem { get; set; }

        [JsonProperty(PropertyName = "isVolatile", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVolatile { get; set; }

        [JsonProperty(PropertyName = "numberOfShards", NullValueHandling = NullValueHandling.Ignore)]
        public int? NumberOfShards { get; set; }

        [JsonProperty(PropertyName = "keyOptions", NullValueHandling = NullValueHandling.Ignore)]
        public KeyOptions KeyOptions { get; set; }

        [JsonProperty(PropertyName = "shardKeys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ShardKeys { get; set; }

        [JsonProperty(PropertyName = "replicationFactor", NullValueHandling = NullValueHandling.Ignore)]
        public int? ReplicationFactor { get; set; }

        [JsonProperty(PropertyName = "shardingStrategy", NullValueHandling = NullValueHandling.Ignore)]
        public string ShardingStrategy { get; set; }
    }
}