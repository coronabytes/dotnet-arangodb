using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoGraphOptions
    {
        /// <summary>
        ///     Only has effect in Enterprise Edition and it is required if isSmart is true. The attribute name that is used to
        ///     smartly shard the vertices of a graph. Every vertex in this SmartGraph has to have this attribute. Cannot be
        ///     modified later.
        /// </summary>
        [JsonPropertyName("smartGraphAttribute")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(PropertyName = "smartGraphAttribute", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string SmartGraphAttribute { get; set; }

        /// <summary>
        ///     The number of shards that is used for every collection within this graph. Cannot be modified later.
        /// </summary>
        [JsonPropertyName("numberOfShards")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(PropertyName = "numberOfShards", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? NumberOfShards { get; set; }

        /// <summary>
        ///     The replication factor used when initially creating collections for this graph. Can be set to "satellite" to create
        ///     a SatelliteGraph, which will ignore numberOfShards, minReplicationFactor and writeConcern (Enterprise Edition
        ///     only).
        /// </summary>
        [JsonPropertyName("replicationFactor")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(PropertyName = "replicationFactor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object ReplicationFactor { get; set; }

        /// <summary>
        ///     Write concern for new collections in the graph. It determines how many copies of each shard are required to be in
        ///     sync on the different DB-Servers. If there are less then these many copies in the cluster a shard will refuse to
        ///     write. Writes to shards with enough up-to-date copies will succeed at the same time however. The value of
        ///     writeConcern can not be larger than replicationFactor. (cluster only)
        /// </summary>
        [JsonPropertyName("writeConcern")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonProperty(PropertyName = "writeConcern", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? WriteConcern { get; set; }
    }
}