using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAccess
    {
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "ro")] ReadOnly,
        [EnumMember(Value = "rw")] ReadWrite
    }
}