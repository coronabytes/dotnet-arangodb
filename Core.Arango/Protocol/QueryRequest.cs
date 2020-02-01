using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
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

    internal class QueryRequestOptions
    {
        [JsonProperty(PropertyName = "fullCount", NullValueHandling = NullValueHandling.Ignore)]
        public bool? FullCount { get; set; }

        [JsonProperty(PropertyName = "optimizer", NullValueHandling = NullValueHandling.Ignore)]
        public QueryOptimizer Optimizer { get; set; }
    }

    internal class QueryOptimizer
    {
        [JsonProperty(PropertyName = "rules", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<string> Rules { get; set; }
    }

    public class ArangoTransactionScope
    {
        [JsonProperty(PropertyName = "read", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Read { get; set; }

        [JsonProperty(PropertyName = "write", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Write { get; set; }
    }

    public class ArangoTransaction
    {
        [JsonProperty(PropertyName = "allowImplicit", NullValueHandling = NullValueHandling.Ignore)]
        public bool AllowImplicit { get; set; }

        [JsonProperty(PropertyName = "collections")]
        public ArangoTransactionScope Collections { get; set; }
    }
}