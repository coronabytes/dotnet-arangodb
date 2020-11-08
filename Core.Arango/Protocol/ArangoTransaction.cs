using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoTransaction
    {

        [JsonPropertyName("allowImplicit")]
        [JsonProperty(PropertyName = "allowImplicit", NullValueHandling = NullValueHandling.Ignore)]
        public bool AllowImplicit { get; set; }

        [JsonPropertyName("collections")]
        [JsonProperty(PropertyName = "collections")]
        public ArangoTransactionScope Collections { get; set; }

        [JsonPropertyName("action")]
        [JsonProperty(PropertyName = "action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WaitForSync { get; set; }

        [JsonPropertyName("lockTimeout")]
        [JsonProperty(PropertyName = "lockTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public int? LockTimeout { get; set; }

        [JsonPropertyName("params")]
        [JsonProperty(PropertyName = "params", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Params { get; set; }

        [JsonPropertyName("maxTransactionSize")]
        [JsonProperty(PropertyName = "maxTransactionSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxTransactionSize { get; set; }
    }
}