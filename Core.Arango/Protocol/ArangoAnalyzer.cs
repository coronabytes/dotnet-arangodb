using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoAnalyzer
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }

        [JsonProperty(PropertyName = "type")] public string Type { get; set; }

        [JsonProperty(PropertyName = "properties")]
        public ArangoAnalyzerProperties Properties { get; set; }

        [JsonProperty(PropertyName = "features")]
        public IList<string> Features { get; set; }
    }
}