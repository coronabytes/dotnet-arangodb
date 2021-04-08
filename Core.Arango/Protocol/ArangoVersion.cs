using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango Server Version and License Information
    /// </summary>
    public class ArangoVersion
    {
        /// <summary>
        ///     Raw version string
        /// </summary>
        [JsonPropertyName("version")]
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        ///     license string
        /// </summary>
        [JsonPropertyName("license")]
        [JsonProperty("license")]
        public string License { get; set; }

        /// <summary>
        ///     Parsed (comparable) version
        /// </summary>
        public Version SemanticVersion { get; set; }
    }
}