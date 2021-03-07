using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango User Functions
    /// </summary>
    public class ArangoFunctionDefinition
    {
        /// <summary>
        ///  The fully qualified name of the user functions.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        /// <summary>
        ///  A string representation of the function body.
        /// </summary>
        [JsonPropertyName("code")]
        [JsonProperty(PropertyName = "code", Required = Required.Always)]
        public string Code { get; set; }

        /// <summary>
        ///  An optional boolean value to indicate whether the function results are fully deterministic (function return value solely depends on the input value and return value is the same for repeated calls with same input).
        ///  The isDeterministic attribute is currently not used but may be used later for optimizations.
        /// </summary>
        [JsonPropertyName("isDeterministic")]
        [JsonProperty(PropertyName = "isDeterministic", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsDeterministic { get; set; }
    }
}