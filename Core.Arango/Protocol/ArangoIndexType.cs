using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango Index Type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoIndexType
    {
        /// <summary>
        ///   Hash - for exact matches
        /// </summary>
        [EnumMember(Value = "hash")] Hash,
        /// <summary>
        ///  Skiplist index - for number/date/string range matches
        /// </summary>
        [EnumMember(Value = "skiplist")] Skiplist,
        /// <summary>
        /// Time to Live index
        /// </summary>
        [EnumMember(Value = "ttl")] Ttl,
        /// <summary>
        ///  Geo-spatial index
        /// </summary>
        [EnumMember(Value = "geo")] Geo,
        /// <summary>
        ///   Fulltext index - Better use ArangoSearch Views
        /// </summary>
        [EnumMember(Value = "fulltext")] Fulltext,
        /// <summary>
        ///  (System) Edge Index
        /// </summary>
        [EnumMember(Value = "edge")] Edge,
        /// <summary>
        ///  (System) Primary Index
        /// </summary>
        [EnumMember(Value = "primary")] Primary
    }
}