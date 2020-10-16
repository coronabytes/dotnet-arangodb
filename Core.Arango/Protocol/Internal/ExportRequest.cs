using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class ExportRequest
    {
        [JsonProperty(PropertyName = "flush", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Flush { get; set; }

        [JsonProperty(PropertyName = "flushWait", NullValueHandling = NullValueHandling.Ignore)]
        public int? FlushWait { get; set; }

        [JsonProperty(PropertyName = "batchSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? BatchSize { get; set; }

        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? Ttl { get; set; }
    }
}