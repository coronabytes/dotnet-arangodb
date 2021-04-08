using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     N-grams for substring matching
    /// </summary>
    public class ArangoEdgeNgram
    {
        /// <summary>
        ///     (Ngram) unsigned integer for the minimum n-gram length
        /// </summary>
        [JsonPropertyName("min")]
        [JsonProperty(PropertyName = "min", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Min { get; set; }

        /// <summary>
        ///     (Ngram) unsigned integer for the maximum n-gram length
        /// </summary>
        [JsonPropertyName("max")]
        [JsonProperty(PropertyName = "max", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Max { get; set; }

        /// <summary>
        ///     (Ngram) to include the original value as well (true) or to produce the n-grams based on min and max only (false)
        /// </summary>
        [JsonPropertyName("preserveOriginal")]
        [JsonProperty(PropertyName = "preserveOriginal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PreserveOriginal { get; set; }

        /// <summary>
        ///     (Ngram) This value will be prepended to n-grams which include the beginning of the input.
        ///     Can be used for matching prefixes.
        ///     Choose a character or sequence as marker which does not occur in the input.
        /// </summary>
        [JsonPropertyName("startMarker")]
        [JsonProperty(PropertyName = "startMarker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StartMarker { get; set; }

        /// <summary>
        ///     (Ngram)  this value will be appended to n-grams which include the end of the input.
        ///     Can be used for matching suffixes.
        ///     Choose a character or sequence as marker which does not occur in the input.
        /// </summary>
        [JsonPropertyName("endMarker")]
        [JsonProperty(PropertyName = "endMarker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string EndMarker { get; set; }

        /// <summary>
        ///     (Ngram) type of the input stream
        /// </summary>
        [JsonPropertyName("streamType")]
        [JsonProperty(PropertyName = "streamType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoNgramStreamType? StreamType { get; set; }
    }
}