using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoVersion
    {
        [JsonPropertyName("version")]
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonPropertyName("license")]
        [JsonProperty("license")]
        public string License { get; set; }
        public Version SemanticVersion { get; set; }
    }
}