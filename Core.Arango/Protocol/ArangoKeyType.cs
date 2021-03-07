using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango key generation type
    /// </summary>
    [JsonConverter(typeof(CamelCaseJsonStringEnumConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoKeyType
    {
        /// <summary>
        ///   The traditional key generator generates numerical keys in ascending order. 
        /// </summary>
        [EnumMember(Value = "traditional")] Traditional,

        /// <summary>
        ///  The autoincrement key generator generates numerical keys in ascending order, the initial offset and the spacing can be configured
        /// </summary>
        [EnumMember(Value = "autoincrement")] Autoincrement,

        /// <summary>
        ///  The uuid key generator generates universally unique 128 bit keys, which are stored in hexadecimal human-readable format.
        ///  This key generator can be used in a single-server or cluster to generate “seemingly random” keys.
        ///  The keys produced by this key generator are not lexicographically sorted.
        /// </summary>
        [EnumMember(Value = "uuid")] Uuid,

        /// <summary>
        ///   The padded key generator generates keys of a fixed length (16 bytes) in ascending lexicographical sort order.
        ///   This is ideal for usage with the RocksDB engine, which will slightly benefit keys that are inserted in lexicographically ascending order.
        ///   The key generator can be used in a single-server or cluster
        /// </summary>
        [EnumMember(Value = "padded")] Padded
    }
}