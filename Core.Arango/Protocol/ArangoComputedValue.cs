using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    /// </summary>
    public class ArangoComputedValue
    {
        /// <summary>
        ///     The name of the target attribute. Can only be a top-level attribute, but you may return a nested object. Cannot be
        ///     _key, _id, _rev, _from, _to, or a shard key attribute.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     An AQL RETURN operation with an expression that computes the desired value.
        /// </summary>
        [JsonPropertyName("expression")]
        [JsonProperty(PropertyName = "expression")]
        public string Expression { get; set; }

        /// <summary>
        ///     Whether the computed value shall take precedence over a user-provided or existing attribute.
        /// </summary>
        [JsonPropertyName("overwrite")]
        [JsonProperty(PropertyName = "overwrite")]
        public bool Overwrite { get; set; }

        /// <summary>
        ///     An array of strings to define on which write operations the value shall be computed.
        /// </summary>
        [JsonPropertyName("computeOn")]
        [JsonProperty(PropertyName = "computeOn", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<ArangoComputeOn> ComputeOn { get; set; }

        /// <summary>
        ///     Whether the target attribute shall be set if the expression evaluates to null. You can set the option to false to
        ///     not set (or unset) the target attribute if the expression returns null. The default is true.
        /// </summary>
        [JsonPropertyName("keepNull")]
        [JsonProperty(PropertyName = "keepNull", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? KeepNull { get; set; }

        /// <summary>
        ///     Whether to let the write operation fail if the expression produces a warning. The default is false.
        /// </summary>
        [JsonPropertyName("failOnWarning")]
        [JsonProperty(PropertyName = "failOnWarning", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? FailOnWarning { get; set; }
    }
}