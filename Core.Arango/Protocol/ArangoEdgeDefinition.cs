using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoEdgeDefinition
    {
        [JsonProperty(PropertyName = "collection")]
        public string Collection { get; set; }

        [JsonProperty(PropertyName = "from")] public IList<string> From { get; set; }

        [JsonProperty(PropertyName = "to")] public IList<string> To { get; set; }
    }
}