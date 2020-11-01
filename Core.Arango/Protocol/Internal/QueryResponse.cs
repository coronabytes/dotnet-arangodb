using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryResponseExtra {
        [JsonProperty(PropertyName = "stats")] 
        public ArangoQueryStatistic Statistic { get; set; }
    }

    internal class QueryResponse<T> : QueryResponseBase<T>
    {
        [JsonProperty(PropertyName = "hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty(PropertyName = "id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "count")] public long Count { get; set; }

        [JsonProperty(PropertyName = "extra")] public QueryResponseExtra Extra { get; set; }
    }
}