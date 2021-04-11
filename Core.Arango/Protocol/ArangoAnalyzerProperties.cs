using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     ArangoSearch Analyzer Properties
    /// </summary>
    public class ArangoAnalyzerProperties : ArangoEdgeNgram
    {
        /// <summary>
        ///     (Stem | Norm | Text) A locale in the format language[_COUNTRY][.encoding][@variant]
        /// </summary>
        /// <example>
        ///     "de.utf-8" or "en_US.utf-8"
        /// </example>
        [JsonPropertyName("locale")]
        [JsonProperty(PropertyName = "locale", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Locale { get; set; }

        /// <summary>
        ///     (Text | Norm) Character conversion
        /// </summary>
        [JsonPropertyName("case")]
        [JsonProperty(PropertyName = "case", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoAnalyzerCase? Case { get; set; }

        /// <summary>
        ///     (Text | Stopwords)
        ///     An array of strings with words to omit from result.
        ///     Default: load words from stopwordsPath.
        ///     To disable stop-word filtering provide an empty array [].
        ///     If both stopwords and stopwordsPath are provided then both word sources are combined.
        /// </summary>
        [JsonPropertyName("stopwords")]
        [JsonProperty(PropertyName = "stopwords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Stopwords { get; set; }

        /// <summary>
        ///     (Text)
        ///     Path with a language sub-directory containing files with words to omit.
        ///     Each word has to be on a separate line.
        ///     Everything after the first whitespace character on a line will be ignored and can be used for comments.
        /// </summary>
        [JsonPropertyName("stopwordsPath")]
        [JsonProperty(PropertyName = "stopwordsPath ", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StopwordsPath { get; set; }

        /// <summary>
        ///     (Text | Norm) To preserve accented characters or to convert accented characters to their base characters (default)
        /// </summary>
        [JsonPropertyName("accent")]
        [JsonProperty(PropertyName = "accent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Accent { get; set; }

        /// <summary>
        ///     (Text) To apply stemming on returned words (default) or to leave the tokenized words as-is.
        /// </summary>
        [JsonPropertyName("stemming")]
        [JsonProperty(PropertyName = "stemming", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Stemming { get; set; }

        /// <summary>
        ///     (Text) If present, then edge n-grams are generated for each token (word)
        /// </summary>
        [JsonPropertyName("edgeNgram")]
        [JsonProperty(PropertyName = "edgeNgram", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoEdgeNgram EdgeNgram { get; set; }

        /// <summary>
        ///     (Delimiter) The delimiting character(s)
        /// </summary>
        [JsonPropertyName("delimiter")]
        [JsonProperty(PropertyName = "delimiter", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Delimiter { get; set; }


        /// <summary>
        ///     (Aql) AQL query to be executed
        /// </summary>
        [JsonPropertyName("queryString")]
        [JsonProperty(PropertyName = "queryString", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string QueryString { get; set; }

        /// <summary>
        ///     (Aql)  true: set the position to 0 for all members of the query result array.  false (default): set the position
        ///     corresponding to the index of the result array member
        /// </summary>
        [JsonPropertyName("collapsePositions")]
        [JsonProperty(PropertyName = "collapsePositions", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CollapsePositions { get; set; }


        /// <summary>
        ///     (Aql)  true (default): treat null like an empty string | false: discard nulls from View index. Can be used for
        ///     index filtering (i.e. make your query return null for unwanted data). Note that empty results are always discarded.
        /// </summary>
        [JsonPropertyName("keepNull")]
        [JsonProperty(PropertyName = "keepNull", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? KeepNull { get; set; }

        /// <summary>
        ///     (Aql) number between 1 and 1000 (default = 1) that determines the batch size for reading data from the query. In
        ///     general, a single token is expected to be returned. However, if the query is expected to return many results, then
        ///     increasing batchSize trades memory for performance.
        /// </summary>
        [JsonPropertyName("batchSize")]
        [JsonProperty(PropertyName = "batchSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? BatchSize { get; set; }

        /// <summary>
        ///     (Aql) memory limit for query execution in bytes. (default is 1048576 = 1Mb) Maximum is 33554432U (32Mb)
        /// </summary>
        [JsonPropertyName("memoryLimit")]
        [JsonProperty(PropertyName = "memoryLimit", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MemoryLimit { get; set; }


        /// <summary>
        ///     (Pipeline) an array of Analyzer definition-like objects with type and properties attributes
        /// </summary>
        [JsonPropertyName("pipeline")]
        [JsonProperty(PropertyName = "pipeline", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<ArangoAnalyzer> Pipeline { get; set; }


        /// <summary>
        ///     (GeoPoint) array of strings that describes the attribute path of the latitude value relative to the field for which
        ///     the Analyzer is defined in the View
        /// </summary>
        [JsonPropertyName("latitude ")]
        [JsonProperty(PropertyName = "latitude", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Latitude { get; set; }

        /// <summary>
        ///     (GeoPoint) array of strings that describes the attribute path of the longitude value relative to the field for
        ///     which the Analyzer is defined in the View
        /// </summary>
        [JsonPropertyName("longitude")]
        [JsonProperty(PropertyName = "longitude", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Longitude { get; set; }

        /// <summary>
        ///     (GeoJson)  (default): index all GeoJSON geometry types (Point, Polygon etc.)
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoAnalyzerGeoJsonType? Type { get; set; }

        /// <summary>
        ///  Overflow properties
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}