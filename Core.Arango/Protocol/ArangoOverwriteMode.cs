using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoOverwriteMode
    {
        /// <summary>
        /// If a document with the specified _key value exists already, nothing will be done and no write operation will be carried out.
        /// The insert operation will return success in this case. This mode does not support returning the old document version using RETURN OLD.
        /// When using RETURN NEW, null will be returned in case the document already existed.
        /// </summary>
        [EnumMember(Value = "ignore")]
        Ignore,
        /// <summary>
        /// If a document with the specified _key value exists already, it will be overwritten with the specified document value.
        /// This mode will also be used when no overwrite mode is specified but the overwrite flag is set to true.
        /// </summary>
        [EnumMember(Value = "replace")]
        Replace,
        /// <summary>
        /// If a document with the specified _key value exists already, it will be patched (partially updated) with the specified document value.
        /// The overwrite mode can be further controlled via the keepNull and mergeObjects parameters.
        /// </summary>
        [EnumMember(Value = "update")]
        Update,
        /// <summary>
        /// If a document with the specified _key value exists already, return a unique constraint violation error so that the insert operation fails.
        /// This is also the default behavior in case the overwrite mode is not set, and the overwrite flag is false or not set either.
        /// </summary>
        [EnumMember(Value = "conflict")]
        Conflict
    }
}