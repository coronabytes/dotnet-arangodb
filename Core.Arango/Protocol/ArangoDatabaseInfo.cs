using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango Database Information
    /// </summary>
    public class ArangoDatabaseInfo
    {
        /// <summary>
        ///   the name of the current database
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///    the id of the current database
        /// </summary>
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        ///   the filesystem path of the current database
        /// </summary>
        [JsonPropertyName("path")]
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        /// <summary>
        ///   whether or not the current database is the _system database
        /// </summary>
        [JsonPropertyName("isSystem")]
        [JsonProperty(PropertyName = "isSystem")]
        public bool IsSystem { get; set; }

        /// <summary>
        ///   the default sharding method for collections created in this database
        /// </summary>
        [JsonPropertyName("sharding")]
        [JsonProperty(PropertyName = "sharding")]
        public string Sharding { get; set; }

        /// <summary>
        ///   the default replication factor for collections in this database
        /// </summary>
        [JsonPropertyName("replicationFactor")]
        [JsonProperty(PropertyName = "replicationFactor")]
        public object ReplicationFactor { get; set; }


        /// <summary>
        ///   the default write concern for collections in this database
        /// </summary>
        [JsonPropertyName("writeConcern")]
        [JsonProperty(PropertyName = "writeConcern")]
        public int? WriteConcern { get; set; }
    }
}