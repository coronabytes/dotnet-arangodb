using System.Runtime.Serialization;
using Core.Arango.Serialization.Json;
using Newtonsoft.Json.Converters;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   AnalyzerType
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumMemberConverter))]
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public enum ArangoAnalyzerType
    {
        /// <summary>
        ///    Treat value as atom (no transformation)
        /// </summary>
        [EnumMember(Value = "identity")]
        Identity,

        /// <summary>
        ///   Split into tokens at user-defined character
        /// </summary>
        [EnumMember(Value = "delimiter")]
        Delimiter,

        /// <summary>
        ///   Apply stemming to the value as a whole
        /// </summary>
        [EnumMember(Value = "stem")]
        Stem,

        /// <summary>
        ///   Apply normalization to the value as a whole
        /// </summary>
        [EnumMember(Value = "norm")]
        Norm,

        /// <summary>
        ///   Create n-grams from value with user-defined lengths
        /// </summary>
        [EnumMember(Value = "ngram")]
        Ngram,

        /// <summary>
        ///   Tokenize into words, optionally with stemming, normalization, stop-word filtering and edge n-gram generation
        /// </summary>
        [EnumMember(Value = "text")]
        Text
    }
}