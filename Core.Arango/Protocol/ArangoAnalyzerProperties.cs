using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoAnalyzerProperties : ArangoEdgeNgram
    {
        [JsonProperty(PropertyName = "locale", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Locale { get; set; }

        [JsonProperty(PropertyName = "case", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Case { get; set; }

        [JsonProperty(PropertyName = "stopwords", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<string> Stopwords { get; set; }

        [JsonProperty(PropertyName = "stopwordsPath ", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StopwordsPath { get; set; }

        [JsonProperty(PropertyName = "accent", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Accent { get; set; }

        [JsonProperty(PropertyName = "stemming", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Stemming { get; set; }

        /// <summary>
        ///  text with ngram
        /// </summary>
        [JsonProperty(PropertyName = "edgeNgram", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ArangoEdgeNgram EdgeNgram { get; set; }

        [JsonProperty(PropertyName = "delimiter", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Delimiter { get; set; }
    }
}