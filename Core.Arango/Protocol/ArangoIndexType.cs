using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoIndexType
    {
        [EnumMember(Value = "hash")] Hash,
        [EnumMember(Value = "skiplist")] Skiplist,
        [EnumMember(Value = "ttl")] TTL,
        [EnumMember(Value = "geo")] Geo,
        [EnumMember(Value = "fulltext")] FullText,
        [EnumMember(Value = "edge")] Edge,
        [EnumMember(Value = "primary")] Primary
    }
}