using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal abstract class QueryResponseBase<T> : ResponseBase
    {
        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }
    }
}