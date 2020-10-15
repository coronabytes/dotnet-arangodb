using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Protocol
{
    internal class QueryResponse<T>: QueryResponseBase<T>
    {
        [JsonProperty(PropertyName = "hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty(PropertyName = "id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "count")] public long Count { get; set; }

        [JsonProperty(PropertyName = "extra")] public JObject Extra { get; set; }
    }

    internal class GraphResponse<T>
    {
        [JsonProperty(PropertyName = "graphs")]
        public List<T> Graphs { get; set; }
    }
}