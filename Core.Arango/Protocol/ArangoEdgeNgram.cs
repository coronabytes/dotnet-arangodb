using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoEdgeNgram 
    {
        [JsonProperty(PropertyName = "min", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Min { get; set; }

        [JsonProperty(PropertyName = "max", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Max { get; set; }

        [JsonProperty(PropertyName = "preserveOriginal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? PreserveOriginal { get; set; }

        [JsonProperty(PropertyName = "startMarker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StartMarker { get; set; }

        [JsonProperty(PropertyName = "endMarker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EndMarker { get; set; }

        [JsonProperty(PropertyName = "streamType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StreamType { get; set; }
    }
}