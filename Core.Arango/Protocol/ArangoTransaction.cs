using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoTransaction
    {
        [JsonProperty(PropertyName = "allowImplicit", NullValueHandling = NullValueHandling.Ignore)]
        public bool AllowImplicit { get; set; }

        [JsonProperty(PropertyName = "collections")]
        public ArangoTransactionScope Collections { get; set; }

        [JsonProperty(PropertyName = "action", NullValueHandling = NullValueHandling.Ignore)]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WaitForSync { get; set; }

        [JsonProperty(PropertyName = "lockTimeout", NullValueHandling = NullValueHandling.Ignore)]
        public int? LockTimeout { get; set; }

        [JsonProperty(PropertyName = "params", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Params { get; set; }

        [JsonProperty(PropertyName = "maxTransactionSize", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxTransactionSize { get; set; }
    }
}