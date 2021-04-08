using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango JavaScript transaction
    /// </summary>
    public class ArangoTransaction
    {
        /// <summary>
        ///     Allow reading from undeclared collections.
        /// </summary>
        [JsonPropertyName("allowImplicit")]
        [JsonProperty(PropertyName = "allowImplicit", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AllowImplicit { get; set; }

        /// <summary>
        ///     Collections must be a JSON object that can have one or all sub-attributes read, write or exclusive, each being an
        ///     array of collection names or a single collection name as string.
        ///     Collections that will be written to in the transaction must be declared with the write or exclusive attribute or it
        ///     will fail, whereas non-declared collections from which is solely read will be added lazily.
        ///     The optional sub-attribute allowImplicit can be set to false to let transactions fail in case of undeclared
        ///     collections for reading.
        ///     Collections for reading should be fully declared if possible, to avoid deadlocks. See locking and isolation for
        ///     more information.
        /// </summary>
        [JsonPropertyName("collections")]
        [JsonProperty(PropertyName = "collections")]
        public ArangoTransactionScope Collections { get; set; }

        /// <summary>
        ///     The actual transaction operations to be executed, in the form of stringified JavaScript code.
        ///     The code will be executed on server side, with late binding.
        ///     It is thus critical that the code specified in action properly sets up all the variables it needs.
        ///     If the code specified in action ends with a return statement, the value returned will also be returned by the REST
        ///     API in the result attribute if the transaction committed successfully.
        /// </summary>
        [JsonPropertyName("action")]
        [JsonProperty(PropertyName = "action", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Action { get; set; }

        /// <summary>
        ///     An optional boolean flag that, if set, will force the transaction to write all data to disk before returning.
        /// </summary>
        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WaitForSync { get; set; }

        /// <summary>
        ///     An optional numeric value that can be used to set a timeout for waiting on collection locks.
        ///     If not specified, a default value will be used.
        ///     Setting lockTimeout to 0 will make ArangoDB not time out waiting for a lock.
        /// </summary>
        [JsonPropertyName("lockTimeout")]
        [JsonProperty(PropertyName = "lockTimeout", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? LockTimeout { get; set; }

        /// <summary>
        ///     Optional arguments passed to action.
        /// </summary>
        [JsonPropertyName("params")]
        [JsonProperty(PropertyName = "params", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, object> Params { get; set; }

        /// <summary>
        ///     Transaction size limit in bytes. Honored by the RocksDB storage engine only.
        /// </summary>
        [JsonPropertyName("maxTransactionSize")]
        [JsonProperty(PropertyName = "maxTransactionSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? MaxTransactionSize { get; set; }
    }
}