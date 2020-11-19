using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [System.Text.Json.Serialization.JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoSchemaLevel
    {
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "new")] New,
        [EnumMember(Value = "moderate")] Moderate,
        [EnumMember(Value = "strict")] Strict
    }
}