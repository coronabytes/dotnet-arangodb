using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoEdgeNgram
    {
        [JsonPropertyName("min")]
        [JsonProperty(PropertyName = "min", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Min { get; set; }

        [JsonPropertyName("max")]
        [JsonProperty(PropertyName = "max", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? Max { get; set; }

        [JsonPropertyName("preserveOriginal")]
        [JsonProperty(PropertyName = "preserveOriginal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? PreserveOriginal { get; set; }

        [JsonPropertyName("startMarker")]
        [JsonProperty(PropertyName = "startMarker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StartMarker { get; set; }

        [JsonPropertyName("endMarker")]
        [JsonProperty(PropertyName = "endMarker", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string EndMarker { get; set; }

        [JsonPropertyName("streamType")]
        [JsonProperty(PropertyName = "streamType", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string StreamType { get; set; }
    }
}