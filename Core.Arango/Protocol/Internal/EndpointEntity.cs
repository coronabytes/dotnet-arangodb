using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class EndpointEntity
    {
        [JsonPropertyName("endpoint")]
        [JsonProperty("endpoint")]
        public string Endpoint { get; set; }
    }
}