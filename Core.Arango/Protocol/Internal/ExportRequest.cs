using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class ExportRequest
    {

        [JsonPropertyName("flush")]
        [JsonProperty(PropertyName = "flush", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Flush { get; set; }

        [JsonPropertyName("flushWait")]
        [JsonProperty(PropertyName = "flushWait", NullValueHandling = NullValueHandling.Ignore)]
        public int? FlushWait { get; set; }

        [JsonPropertyName("batchSize")]
        [JsonProperty(PropertyName = "batchSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? BatchSize { get; set; }

        [JsonPropertyName("ttl")]
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? Ttl { get; set; }
    }
}