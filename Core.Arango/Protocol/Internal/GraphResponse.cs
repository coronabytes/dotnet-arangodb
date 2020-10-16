using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class GraphResponse<T>
    {
        [JsonProperty(PropertyName = "graphs")]
        public List<T> Graphs { get; set; }
    }
}