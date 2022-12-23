using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoComputeOn
    {
        /// <summary>
        ///     Insert
        /// </summary>
        [EnumMember(Value = "insert")] Insert,

        /// <summary>
        ///     Update
        /// </summary>
        [EnumMember(Value = "update")] Update,

        /// <summary>
        ///     Replace
        /// </summary>
        [EnumMember(Value = "replace")] Replace
    }
}