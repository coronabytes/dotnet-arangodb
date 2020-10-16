using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryRequest
    {
        [JsonProperty(PropertyName = "query")] public string Query { get; set; }

        [JsonProperty(PropertyName = "bindVars", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, object> BindVars { get; set; }

        [JsonProperty(PropertyName = "count", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Count { get; set; }

        [JsonProperty(PropertyName = "batchSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? BatchSize { get; set; }

        [JsonProperty(PropertyName = "memoryLimit", NullValueHandling = NullValueHandling.Ignore)]
        public int? MemoryLimit { get; set; }

        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TTL { get; set; }

        [JsonProperty(PropertyName = "cache", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Cache { get; set; }

        [JsonProperty(PropertyName = "options", NullValueHandling = NullValueHandling.Ignore)]
        public QueryRequestOptions Options { get; set; }
    }
}