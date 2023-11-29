using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     ArangoSearch Analyzer
    /// </summary>
    public class ArangoAnalyzer
    {
        /// <summary>
        ///     The Analyzer name.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; }


        /// <summary>
        ///     The Analyzer type.
        /// </summary>
        /// <example>
        /// </example>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public ArangoAnalyzerType Type { get; set; }

        /// <summary>
        ///     The properties used to configure the specified Analyzer type.
        /// </summary>
        [JsonPropertyName("properties")]
        [JsonProperty(PropertyName = "properties")]
        public ArangoAnalyzerProperties Properties { get; set; }

        /// <summary>
        ///     The set of features to set on the Analyzer generated fields. The default value is an empty array.
        /// </summary>
        [JsonPropertyName("features")]
        [JsonProperty(PropertyName = "features")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Features { get; set; }

        /// <summary>
        ///  Overflow properties
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}