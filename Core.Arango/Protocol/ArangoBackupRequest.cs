using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoBackupRequest
    {
        [JsonPropertyName("label")]
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }

        [JsonPropertyName("timeout")]
        [JsonProperty(PropertyName = "timeout")]
        public int Timeout { get; set; }

        [JsonPropertyName("allowInconsistent")]
        [JsonProperty(PropertyName = "allowInconsistent")]
        public bool AllowInconsistent { get; set; }

        [JsonPropertyName("force")]
        [JsonProperty(PropertyName = "force")]
        public bool Force { get; set; }
    }
}