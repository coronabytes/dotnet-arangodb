using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization;
using Core.Arango.Serialization.System;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAccess
    {
        [EnumMember(Value = "none")] None,
        [EnumMember(Value = "ro")] ReadOnly,
        [EnumMember(Value = "rw")] ReadWrite
    }
}