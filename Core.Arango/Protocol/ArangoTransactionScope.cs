using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoTransactionScope
    {
        [JsonPropertyName("read")]
        [JsonProperty(PropertyName = "read", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Read { get; set; }

        [JsonPropertyName("write")]
        [JsonProperty(PropertyName = "write", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Write { get; set; }
    }
}