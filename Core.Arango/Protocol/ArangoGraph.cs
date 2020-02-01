using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoGraph
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "edgeDefinitions")]
        public IList<ArangoEdgeDefinition> EdgeDefinitions { get; set; }

        [JsonProperty(PropertyName = "isSmart", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsSmart { get; set; }
    }
}