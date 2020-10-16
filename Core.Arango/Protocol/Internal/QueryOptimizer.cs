using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryOptimizer
    {
        [JsonProperty(PropertyName = "rules", NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<string> Rules { get; set; }
    }
}