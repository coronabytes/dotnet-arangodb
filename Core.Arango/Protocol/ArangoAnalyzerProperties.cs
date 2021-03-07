using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   ArangoSearch Analyzer Properties
    /// </summary>
    public class ArangoAnalyzerProperties : ArangoEdgeNgram
    {
        /// <summary>
        ///   (Stem | Norm | Text) A locale in the format language[_COUNTRY][.encoding][@variant]
        /// </summary>
        /// <example>
        ///  "de.utf-8" or "en_US.utf-8"
        /// </example>
        [JsonPropertyName("locale")]
        [JsonProperty(PropertyName = "locale", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Locale { get; set; }

        /// <summary>
        ///   (Text | Norm) Character conversion
        /// </summary>
        [JsonPropertyName("case")]
        [JsonProperty(PropertyName = "case", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoAnalyzerCase? Case { get; set; }

        /// <summary>
        ///   (Text)
        ///   An array of strings with words to omit from result.
        ///   Default: load words from stopwordsPath.
        ///   To disable stop-word filtering provide an empty array [].
        ///   If both stopwords and stopwordsPath are provided then both word sources are combined.
        /// </summary>
        [JsonPropertyName("stopwords")]
        [JsonProperty(PropertyName = "stopwords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Stopwords { get; set; }

        /// <summary>
        ///   (Text)
        ///   Path with a language sub-directory containing files with words to omit.
        ///   Each word has to be on a separate line.
        ///   Everything after the first whitespace character on a line will be ignored and can be used for comments.
        /// </summary>
        [JsonPropertyName("stopwordsPath")]
        [JsonProperty(PropertyName = "stopwordsPath ", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StopwordsPath { get; set; }

        /// <summary>
        ///   (Text | Norm) To preserve accented characters or to convert accented characters to their base characters (default)
        /// </summary>
        [JsonPropertyName("accent")]
        [JsonProperty(PropertyName = "accent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Accent { get; set; }

        /// <summary>
        ///   (Text) To apply stemming on returned words (default) or to leave the tokenized words as-is.
        /// </summary>
        [JsonPropertyName("stemming")]
        [JsonProperty(PropertyName = "stemming", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Stemming { get; set; }

        /// <summary>
        ///   (Text) If present, then edge n-grams are generated for each token (word)
        /// </summary>
        [JsonPropertyName("edgeNgram")]
        [JsonProperty(PropertyName = "edgeNgram", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoEdgeNgram EdgeNgram { get; set; }

        /// <summary>
        ///   (Delimiter) The delimiting character(s)
        /// </summary>
        [JsonPropertyName("delimiter")]
        [JsonProperty(PropertyName = "delimiter", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Delimiter { get; set; }
    }
}