using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoBackup
    {
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonPropertyName("version")]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonPropertyName("keys")]
        [JsonProperty(PropertyName = "keys")]
        public List<string> Keys { get; set; }

        [JsonPropertyName("sizeInBytes")]
        [JsonProperty(PropertyName = "sizeInBytes")]
        public long SizeInBytes { get; set; }

        [JsonPropertyName("nrFiles")]
        [JsonProperty(PropertyName = "nrFiles")]
        public long NrFiles { get; set; }

        [JsonPropertyName("nrDBServers")]
        [JsonProperty(PropertyName = "nrDBServers")]
        public long NrDBServers { get; set; }

        [JsonPropertyName("available")]
        [JsonProperty(PropertyName = "available")]
        public bool Available { get; set; }

        [JsonPropertyName("nrPiecesPresent")]
        [JsonProperty(PropertyName = "nrPiecesPresent")]
        public long NrPiecesPresent { get; set; }

        [JsonPropertyName("potentiallyInconsistent")]
        [JsonProperty(PropertyName = "potentiallyInconsistent")]
        public bool PotentiallyInconsistent { get; set; }
    }
}