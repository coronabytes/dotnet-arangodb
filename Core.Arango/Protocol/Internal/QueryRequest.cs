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
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object> BindVars { get; set; }

        [JsonPropertyName("count")]
        [JsonProperty(PropertyName = "count", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Count { get; set; }

        [JsonPropertyName("batchSize")]
        [JsonProperty(PropertyName = "batchSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? BatchSize { get; set; }

        [JsonPropertyName("memoryLimit")]
        [JsonProperty(PropertyName = "memoryLimit", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MemoryLimit { get; set; }

        [JsonPropertyName("ttl")]
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? TTL { get; set; }

        [JsonPropertyName("cache")]
        [JsonProperty(PropertyName = "cache", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Cache { get; set; }

        [JsonPropertyName("options")]
        [JsonProperty(PropertyName = "options", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]

        public QueryRequestOptions Options { get; set; }
    }
}