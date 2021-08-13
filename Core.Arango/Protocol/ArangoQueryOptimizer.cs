using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Optimizer options
    /// </summary>
    public class ArangoQueryOptimizer
    {
        /// <summary>
        ///     A list of to-be-included or to-be-excluded optimizer rules can be put into this attribute, telling the optimizer to
        ///     include or exclude specific rules. To disable a rule, prefix its name with a -, to enable a rule, prefix it with a
        ///     +. There is also a pseudo-rule all, which matches all optimizer rules. -all disables all rules.
        /// </summary>
        [JsonPropertyName("rules")]
        [JsonProperty(PropertyName = "rules", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<string> Rules { get; set; }

        /// <summary>
        ///     If set to true or 1, then the additional query profiling information will be returned in the sub-attribute profile
        ///     of the extra return attribute, if the query result is not served from the query cache. Set to 2 the query will
        ///     include execution stats per query plan node in sub-attribute stats.nodes of the extra return attribute.
        ///     Additionally the query plan is returned in the sub-attribute extra.plan.
        /// </summary>
        [JsonPropertyName("profile")]
        [JsonProperty(PropertyName = "profile", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Profile { get; set; }

        /// <summary>
        ///     This Enterprise Edition parameter allows to configure how long a DB-Server will have time to bring the
        ///     SatelliteCollections involved in the query into sync. The default value is 60.0 (seconds). When the max time has
        ///     been reached the query will be stopped.
        /// </summary>
        [JsonPropertyName("satelliteSyncWait")]
        [JsonProperty(PropertyName = "satelliteSyncWait", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? SatelliteSyncWait { get; set; }

        /// <summary>
        ///     The query has to be executed within the given runtime or it will be killed. The value is specified in seconds. The
        ///     default value is 0.0 (no timeout).
        /// </summary>
        [JsonPropertyName("maxRuntime")]
        [JsonProperty(PropertyName = "maxRuntime", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? MaxRuntime { get; set; }

        /// <summary>
        ///     Transaction size limit in bytes. Honored by the RocksDB storage engine only.
        /// </summary>
        [JsonPropertyName("maxTransactionSize")]
        [JsonProperty(PropertyName = "maxTransactionSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? MaxTransactionSize { get; set; }

        /// <summary>
        ///     Maximum total size of operations after which an intermediate commit is performed automatically. Honored by the
        ///     RocksDB storage engine only.
        /// </summary>
        [JsonPropertyName("intermediateCommitSize")]
        [JsonProperty(PropertyName = "intermediateCommitSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? IntermediateCommitSize { get; set; }

        /// <summary>
        ///     Maximum number of operations after which an intermediate commit is performed automatically. Honored by the RocksDB
        ///     storage engine only.
        /// </summary>
        [JsonPropertyName("intermediateCommitCount")]
        [JsonProperty(PropertyName = "intermediateCommitCount", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? IntermediateCommitCount { get; set; }

        /// <summary>
        ///     AQL queries (especially graph traversals) will treat collection to which a user has no access rights as if these
        ///     collections were empty.
        ///     Instead of returning a forbidden access error, your queries will execute normally.
        ///     This is intended to help with certain use-cases: A graph contains several collections and different users execute
        ///     AQL queries on that graph.
        ///     You can now naturally limit the accessible results by changing the access rights of users on collections.
        ///     This feature is only available in the Enterprise Edition.
        /// </summary>
        [JsonPropertyName("skipInaccessibleCollections")]
        [JsonProperty(PropertyName = "skipInaccessibleCollections", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SkipInaccessibleCollections { get; set; }
    }
}