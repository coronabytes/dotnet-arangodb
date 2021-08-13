using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Object describing the query and query parameters.
    /// </summary>
    public class ArangoCursor
    {
        /// <summary>
        ///     contains the query string to be executed
        /// </summary>
        [JsonPropertyName("query")]
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }

        /// <summary>
        ///     key/value pairs representing the bind parameters.
        /// </summary>
        [JsonPropertyName("bindVars")]
        [JsonProperty(PropertyName = "bindVars", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, object> BindVars { get; set; }

        /// <summary>
        ///     indicates whether the number of documents in the result set should be returned in the “count” attribute of the
        ///     result. Calculating the “count” attribute might have a performance impact for some queries in the future so this
        ///     option is turned off by default, and “count” is only returned when requested.
        /// </summary>
        [JsonPropertyName("count")]
        [JsonProperty(PropertyName = "count", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Count { get; set; }

        /// <summary>
        ///     maximum number of result documents to be transferred from the server to the client in one roundtrip. If this
        ///     attribute is not set, a server-controlled default value will be used. A batchSize value of 0 is disallowed.
        /// </summary>
        [JsonPropertyName("batchSize")]
        [JsonProperty(PropertyName = "batchSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? BatchSize { get; set; }

        /// <summary>
        ///     the maximum number of memory (measured in bytes) that the query is allowed to use. If set, then the query will fail
        ///     with error “resource limit exceeded” in case it allocates too much memory. A value of 0 indicates that there is no
        ///     memory limit.
        /// </summary>
        [JsonPropertyName("memoryLimit")]
        [JsonProperty(PropertyName = "memoryLimit", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? MemoryLimit { get; set; }

        /// <summary>
        ///     The time-to-live for the cursor (in seconds). If the result set is small enough (less than or equal to batchSize)
        ///     then results are returned right away. Otherwise they are stored in memory and will be accessible via the cursor
        ///     with respect to the ttl. The cursor will be removed on the server automatically after the specified amount of time.
        ///     This is useful to ensure garbage collection of cursors that are not fully fetched by clients. If not set, a
        ///     server-defined value will be used (default: 30 seconds).
        /// </summary>
        [JsonPropertyName("ttl")]
        [JsonProperty(PropertyName = "ttl", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public double? TTL { get; set; }

        /// <summary>
        ///     flag to determine whether the AQL query results cache shall be used. If set to false, then any query cache lookup
        ///     will be skipped for the query. If set to true, it will lead to the query cache being checked for the query if the
        ///     query cache mode is either on or demand.
        /// </summary>
        [JsonPropertyName("cache")]
        [JsonProperty(PropertyName = "cache", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Cache { get; set; }

        /// <summary>
        ///     key/value object with extra options for the query.
        /// </summary>
        [JsonPropertyName("options")]
        [JsonProperty(PropertyName = "options", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoQueryOptions Options { get; set; }
    }
}