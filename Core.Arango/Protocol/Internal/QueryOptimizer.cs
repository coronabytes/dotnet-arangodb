using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryOptimizer
    {
        [JsonPropertyName("rules")]
        [JsonProperty(PropertyName = "rules", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<string> Rules { get; set; }
    }
}