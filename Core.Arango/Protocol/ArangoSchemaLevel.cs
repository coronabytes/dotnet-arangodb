using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango schema validation level
    /// </summary>
    [JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoSchemaLevel
    {
        /// <summary>
        ///   The rule is inactive and validation thus turned off.
        /// </summary>
        [EnumMember(Value = "none")] None,
        /// <summary>
        ///   Only newly inserted documents are validated.
        /// </summary>
        [EnumMember(Value = "new")] New,
        /// <summary>
        ///   New and modified documents must pass validation, except for modified documents where the OLD value did not pass validation already.
        ///   This level is useful if you have documents which do not match your target structure, but you want to stop the insertion of more invalid documents and prohibit that valid documents are changed to invalid documents.
        /// </summary>
        [EnumMember(Value = "moderate")] Moderate,
        /// <summary>
        ///   All new and modified document must strictly pass validation. No exceptions are made (default).
        /// </summary>
        [EnumMember(Value = "strict")] Strict
    }
}