using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Schema Validation
    /// </summary>
    public class ArangoSchema
    {
        /// <summary>
        ///     The rule attribute must contain the JSON Schema description.
        /// </summary>
        /// <example>
        ///     {
        ///     properties: { nums: { type: "array", items: { type: "number", maximum: 6 } } },
        ///     additionalProperties: { type: "string" },
        ///     required: ["nums"]
        ///     }
        /// </example>
        [JsonPropertyName("rule")]
        [JsonProperty(PropertyName = "rule")]
        public object Rule { get; set; }

        /// <summary>
        ///     controls when the validation will be applied.
        /// </summary>
        [JsonPropertyName("level")]
        [JsonProperty(PropertyName = "level")]
        public ArangoSchemaLevel Level { get; set; } = ArangoSchemaLevel.Strict;

        /// <summary>
        ///     sets the message that will be used when validation fails.
        /// </summary>
        [JsonPropertyName("message")]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}