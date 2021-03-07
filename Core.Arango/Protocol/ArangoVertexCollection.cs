using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoVertexCollection
    {
        [JsonPropertyName("collection")]
        [JsonProperty(PropertyName = "collection")]
        public string Collection { get; set; }
    }
}