using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryResponse<T> : QueryResponseBase<T>
    {
        [JsonPropertyName("hasMore")]
        [JsonProperty(PropertyName = "hasMore")]
        public bool HasMore { get; set; }

        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")] 
        public string Id { get; set; }

        [JsonPropertyName("count")]
        [JsonProperty(PropertyName = "count")] 
        public long Count { get; set; }

        [JsonPropertyName("extra")]
        [JsonProperty(PropertyName = "extra")] 
        public QueryResponseExtra Extra { get; set; }
    }
}