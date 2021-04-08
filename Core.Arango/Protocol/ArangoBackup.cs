using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango HotBackup
    /// </summary>
    public class ArangoBackup
    {
        /// <summary>
        ///     Unique identifier
        /// </summary>
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        ///     ArangoDB version
        /// </summary>
        [JsonPropertyName("version")]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        /// <summary>
        ///     Keys
        /// </summary>
        [JsonPropertyName("keys")]
        [JsonProperty(PropertyName = "keys")]
        public List<string> Keys { get; set; }

        /// <summary>
        ///     SizeInBytes
        /// </summary>
        [JsonPropertyName("sizeInBytes")]
        [JsonProperty(PropertyName = "sizeInBytes")]
        public long SizeInBytes { get; set; }

        /// <summary>
        ///     Number of files
        /// </summary>
        [JsonPropertyName("nrFiles")]
        [JsonProperty(PropertyName = "nrFiles")]
        public long NrFiles { get; set; }

        /// <summary>
        ///     Number of database servers
        /// </summary>
        [JsonPropertyName("nrDBServers")]
        [JsonProperty(PropertyName = "nrDBServers")]
        public long NrDbServers { get; set; }

        /// <summary>
        ///     is available?
        /// </summary>
        [JsonPropertyName("available")]
        [JsonProperty(PropertyName = "available")]
        public bool Available { get; set; }

        /// <summary>
        ///     Number of pieces present
        /// </summary>
        [JsonPropertyName("nrPiecesPresent")]
        [JsonProperty(PropertyName = "nrPiecesPresent")]
        public long NrPiecesPresent { get; set; }

        /// <summary>
        ///     Is potentially inconsistent?
        /// </summary>
        [JsonPropertyName("potentiallyInconsistent")]
        [JsonProperty(PropertyName = "potentiallyInconsistent")]
        public bool PotentiallyInconsistent { get; set; }
    }
}