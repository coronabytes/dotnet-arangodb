using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoIndexType
    {
        [EnumMember(Value = "hash")] Hash,
        [EnumMember(Value = "skiplist")] Skiplist,
        [EnumMember(Value = "ttl")] TTL,
        [EnumMember(Value = "geo")] Geo,
        [EnumMember(Value = "fulltext")] FullText
    }
}