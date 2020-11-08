using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoAnalyzerProperties : ArangoEdgeNgram
    {
        [JsonPropertyName("locale")]
        [JsonProperty(PropertyName = "locale", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Locale { get; set; }

        [JsonPropertyName("case")]
        [JsonProperty(PropertyName = "case", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Case { get; set; }

        [JsonPropertyName("stopwords")]
        [JsonProperty(PropertyName = "stopwords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<string> Stopwords { get; set; }

        [JsonPropertyName("stopwordsPath")]
        [JsonProperty(PropertyName = "stopwordsPath ", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StopwordsPath { get; set; }

        [JsonPropertyName("accent")]
        [JsonProperty(PropertyName = "accent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Accent { get; set; }

        [JsonPropertyName("stemming")]
        [JsonProperty(PropertyName = "stemming", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Stemming { get; set; }

        [JsonPropertyName("edgeNgram")]
        [JsonProperty(PropertyName = "edgeNgram", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ArangoEdgeNgram EdgeNgram { get; set; }

        [JsonPropertyName("delimiter")]
        [JsonProperty(PropertyName = "delimiter", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Delimiter { get; set; }
    }
}