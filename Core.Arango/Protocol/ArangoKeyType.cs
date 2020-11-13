using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoKeyType
    {
        [EnumMember(Value = "traditional")] Traditional,
        [EnumMember(Value = "autoincrement")] Autoincrement,
        [EnumMember(Value = "uuid")] Uuid,
        [EnumMember(Value = "padded")] Padded
    }
}