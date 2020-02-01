using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Protocol
{
    internal class QueryResponse<T>
    {
        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }

        [JsonProperty(PropertyName = "hasMore")]
        public bool HasMore { get; set; }

        [JsonProperty(PropertyName = "id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "count")] public long Count { get; set; }

        [JsonProperty(PropertyName = "code")] public short Code { get; set; }
        [JsonProperty(PropertyName = "error")] public bool Error { get; set; }

        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty(PropertyName = "extra")] public JObject Extra { get; set; }
    }

    internal class GraphResponse<T>
    {
        [JsonProperty(PropertyName = "graphs")]
        public List<T> Graphs { get; set; }
    }
}