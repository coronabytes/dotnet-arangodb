using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango Database Options
    /// </summary>
    public class ArangoDatabaseOptions
    {
        /// <summary>
        /// The sharding method to use for new collections in this database.
        /// Valid values are: “”, “flexible”, or “single”.
        /// The first two are equivalent. (cluster only)
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Sharding { get; set; }

        /// <summary>
        /// Default replication factor for new collections created in this database.
        /// Special values include “satellite”, which will replicate the collection to every DB-Server (Enterprise Edition only), and 1, which disables replication. (cluster only)
        /// </summary>
        [JsonPropertyName("replicationFactor")]
        [JsonProperty(PropertyName = "replicationFactor")]
        public object ReplicationFactor { get; set; }


        /// <summary>
        /// Default write concern for new collections created in this database.
        /// It determines how many copies of each shard are required to be in sync on the different DB-Servers.
        /// If there are less then these many copies in the cluster a shard will refuse to write.
        /// Writes to shards with enough up-to-date copies will succeed at the same time however.
        /// The value of writeConcern can not be larger than replicationFactor. (cluster only)
        /// </summary>
        [JsonPropertyName("writeConcern")]
        [JsonProperty(PropertyName = "writeConcern")]
        public int? WriteConcern { get; set; }
    }
}