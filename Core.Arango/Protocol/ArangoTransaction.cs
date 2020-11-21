using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoTransaction
    {

        [JsonPropertyName("allowImplicit")]
        [JsonProperty(PropertyName = "allowImplicit", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AllowImplicit { get; set; }

        [JsonPropertyName("collections")]
        [JsonProperty(PropertyName = "collections")]
        public ArangoTransactionScope Collections { get; set; }

        [JsonPropertyName("action")]
        [JsonProperty(PropertyName = "action", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Action { get; set; }

        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WaitForSync { get; set; }

        [JsonPropertyName("lockTimeout")]
        [JsonProperty(PropertyName = "lockTimeout", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LockTimeout { get; set; }

        [JsonPropertyName("params")]
        [JsonProperty(PropertyName = "params", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object> Params { get; set; }

        [JsonPropertyName("maxTransactionSize")]
        [JsonProperty(PropertyName = "maxTransactionSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxTransactionSize { get; set; }
    }
}