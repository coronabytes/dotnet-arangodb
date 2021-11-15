using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     ArangoGeoJsonType
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAnalyzerBreakType
    {
        /// <summary>
        ///     return all tokens
        /// </summary>
        [EnumMember(Value = "all")] All,

        /// <summary>
        ///      return tokens composed of alphanumeric characters only (default)
        /// </summary>
        [EnumMember(Value = "alpha")] Alpha,

        /// <summary>
        ///     return tokens composed of non-whitespace characters only. Note that the list of whitespace characters does not include line breaks:
        /// </summary>
        [EnumMember(Value = "graphic")] Graphic
    }
}