using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public abstract class QueryResponseBase<T>: ArangoResponseBase
    {
        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }
    }
}