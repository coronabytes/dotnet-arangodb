using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoAnalyzerProperties : ArangoEdgeNgram
    {
        [JsonPropertyName("locale")]
        [JsonProperty(PropertyName = "locale", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Locale { get; set; }

        [JsonPropertyName("case")]
        [JsonProperty(PropertyName = "case", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Case { get; set; }

        [JsonPropertyName("stopwords")]
        [JsonProperty(PropertyName = "stopwords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Stopwords { get; set; }

        [JsonPropertyName("stopwordsPath")]
        [JsonProperty(PropertyName = "stopwordsPath ", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StopwordsPath { get; set; }

        [JsonPropertyName("accent")]
        [JsonProperty(PropertyName = "accent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Accent { get; set; }

        [JsonPropertyName("stemming")]
        [JsonProperty(PropertyName = "stemming", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Stemming { get; set; }

        [JsonPropertyName("edgeNgram")]
        [JsonProperty(PropertyName = "edgeNgram", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoEdgeNgram EdgeNgram { get; set; }

        [JsonPropertyName("delimiter")]
        [JsonProperty(PropertyName = "delimiter", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Delimiter { get; set; }
    }
}