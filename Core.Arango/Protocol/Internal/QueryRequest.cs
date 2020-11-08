using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryRequest
    {
        [JsonPropertyName("query")]
        [JsonProperty(PropertyName = "query")] public string Query { get; set; }


        [JsonPropertyName("bindVars")]
        [JsonProperty(PropertyName = "bindVars", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, object> BindVars { get; set; }

        [JsonPropertyName("count")]
        [JsonProperty(PropertyName = "count", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Count { get; set; }

        [JsonPropertyName("batchSize")]
        [JsonProperty(PropertyName = "batchSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? BatchSize { get; set; }

        [JsonPropertyName("memoryLimit")]
        [JsonProperty(PropertyName = "memoryLimit", NullValueHandling = NullValueHandling.Ignore)]
        public int? MemoryLimit { get; set; }

        [JsonPropertyName("ttl")]
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TTL { get; set; }

        [JsonPropertyName("cache")]
        [JsonProperty(PropertyName = "cache", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Cache { get; set; }

        [JsonPropertyName("options")]
        [JsonProperty(PropertyName = "options", NullValueHandling = NullValueHandling.Ignore)]
        public QueryRequestOptions Options { get; set; }
    }
}