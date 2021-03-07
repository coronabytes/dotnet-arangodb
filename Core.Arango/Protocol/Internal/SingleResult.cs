using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class SingleResult<T>
    {
        [JsonPropertyName("result")]
        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }
    }
}