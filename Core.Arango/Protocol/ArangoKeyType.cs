using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoKeyType
    {
        [EnumMember(Value = "traditional")] Traditional,
        [EnumMember(Value = "autoincrement")] AutoIncrement,
        [EnumMember(Value = "uuid")] Uuid,
        [EnumMember(Value = "padded")] Padded
    }
}