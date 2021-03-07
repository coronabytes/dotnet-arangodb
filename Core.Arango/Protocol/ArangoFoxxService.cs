using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Foxx Service Description
    /// </summary>
    public class ArangoFoxxService
    {
        /// <summary>
        ///    the mount path of the service
        /// </summary>
        [JsonPropertyName("mount")]
        [JsonProperty(PropertyName = "mount")]
        public string Mount { get; set; }

        /// <summary>
        ///  true if the service is running in development mode
        /// </summary>
        [JsonPropertyName("development")]
        [JsonProperty(PropertyName = "development")]
        public bool Development { get; set; }

        /// <summary>
        ///  true if the service is running in 2.8 legacy compatibility mode
        /// </summary>
        [JsonPropertyName("legacy")]
        [JsonProperty(PropertyName = "legacy")]
        public bool Legacy { get; set; }

        /// <summary>
        ///  a string identifying the service type
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///  a semver-compatible version string
        /// </summary>
        [JsonPropertyName("version")]
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}