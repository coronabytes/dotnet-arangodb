using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango Index Type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoIndexType
    {
        /// <summary>
        ///     A hash index is an unsorted index that can be used to find individual documents by equality lookups.
        /// </summary>
        [EnumMember(Value = "hash")] Hash,

        /// <summary>
        ///     A skiplist is a sorted index that can be used to find individual documents or ranges of documents.
        /// </summary>
        [EnumMember(Value = "skiplist")] Skiplist,

        /// <summary>
        ///     A persistent index is a sorted index that can be used for finding individual documents or ranges of documents.
        ///     In contrast to the other indexes, the contents of a persistent index are stored on disk and thus do not need to be
        ///     rebuilt in memory from the documents when the collection is loaded.
        /// </summary>
        [EnumMember(Value = "persistent")] Persistent,

        /// <summary>
        ///     The TTL index can be used for automatically removing expired documents from a collection.
        ///     Documents which are expired are eventually removed by a background thread.
        /// </summary>
        [EnumMember(Value = "ttl")] Ttl,

        /// <summary>
        ///     Geo-spatial index
        /// </summary>
        [EnumMember(Value = "geo")] Geo,

        /// <summary>
        ///     A fulltext index can be used to find words, or prefixes of words inside documents.
        ///     A fulltext index can be set on one attribute only, and will index all words contained in documents that have a
        ///     textual value in this attribute.
        ///     Only words with a (specifiable) minimum length are indexed. Word tokenization is done using the word boundary
        ///     analysis provided by libicu, which is taking into account the selected language provided at server start. Words are
        ///     indexed in their lower-cased form.
        ///     The index supports complete match queries (full words) and prefix queries.
        /// </summary>
        [EnumMember(Value = "fulltext")] Fulltext,

        /// <summary>
        /// 
        /// </summary>
        [EnumMember(Value = "zkd")] MultiDimensional,

        /// <summary>
        ///     An edge index is automatically created for edge collections.
        ///     It contains connections between vertex documents and is invoked when the connecting edges of a vertex are queried.
        ///     There is no way to explicitly create or delete edge indexes.
        ///     The edge index is non-unique.
        /// </summary>
        [EnumMember(Value = "edge")] Edge,

        /// <summary>
        ///     A primary index is automatically created for each collections.
        ///     It indexes the documents’ primary keys, which are stored in the _key system attribute.
        ///     The primary index is unique and can be used for queries on both the _key and _id attributes.
        ///     There is no way to explicitly create or delete primary indexes.
        /// </summary>
        [EnumMember(Value = "primary")] Primary
    }
}