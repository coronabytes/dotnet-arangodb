using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoGraph
    {

        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")] 
        public string Name { get; set; }

        [JsonPropertyName("edgeDefinitions")]
        [JsonProperty(PropertyName = "edgeDefinitions")]
        public IList<ArangoEdgeDefinition> EdgeDefinitions { get; set; }

        [JsonPropertyName("isSmart")]
        [JsonProperty(PropertyName = "isSmart", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsSmart { get; set; }
    }
}