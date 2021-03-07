using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   N-gram stream type
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoNgramStreamType
    {
        /// <summary>
        ///    One byte is considered as one character (default)
        /// </summary>
        [EnumMember(Value = "binary")] Binary,

        /// <summary>
        ///    one Unicode codepoint is treated as one character
        /// </summary>
        [EnumMember(Value = "upper")] Utf8,
    }
}