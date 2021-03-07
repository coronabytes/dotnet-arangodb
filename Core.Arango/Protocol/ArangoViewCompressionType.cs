using System.Runtime.Serialization;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango View Compression
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoViewCompressionType
    {
        /// <summary>
        ///    LZ4
        /// </summary>
        [EnumMember(Value = "lz4")] Lz4,
        
        /// <summary>
        ///    None
        /// </summary>
        [EnumMember(Value = "none")] None,
    }
}