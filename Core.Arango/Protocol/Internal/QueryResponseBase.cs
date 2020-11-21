using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal abstract class QueryResponseBase<T> : ResponseBase
    {
        [JsonPropertyName("result")]
        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }
    }

    internal class SingleResult<T>
    {
        [JsonPropertyName("result")]
        [JsonProperty(PropertyName = "result")]
        public T Result { get; set; }
    }
}