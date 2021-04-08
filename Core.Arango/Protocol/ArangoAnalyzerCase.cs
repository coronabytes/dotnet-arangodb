using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     AnalyzerType
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAnalyzerCase
    {
        /// <summary>
        ///     To convert to all lower-case characters (default)
        /// </summary>
        [EnumMember(Value = "lower")] Lower,

        /// <summary>
        ///     To convert to all upper-case characters
        /// </summary>
        [EnumMember(Value = "upper")] Upper,

        /// <summary>
        ///     To not change character case
        /// </summary>
        [EnumMember(Value = "none")] None
    }
}