using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoTransactionScope
    {
        [JsonProperty(PropertyName = "read", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Read { get; set; }

        [JsonProperty(PropertyName = "write", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Write { get; set; }
    }
}