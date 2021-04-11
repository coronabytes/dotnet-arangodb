using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class GraphsResponse<T>
    {
        [JsonPropertyName("graphs")]
        [JsonProperty(PropertyName = "graphs")]
        public List<T> Graphs { get; set; }
    }

    internal class GraphResponse<T>
    {
        [JsonPropertyName("graph")]
        [JsonProperty(PropertyName = "graph")]
        public T Graph { get; set; }
    }


}