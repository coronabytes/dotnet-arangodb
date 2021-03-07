using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango Transaction Scope
    /// </summary>
    public class ArangoTransactionScope
    {
        /// <summary>
        ///  Collections to read from
        /// </summary>
        [JsonPropertyName("read")]
        [JsonProperty(PropertyName = "read", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Read { get; set; }

        /// <summary>
        ///  Collections to write to
        /// </summary>
        [JsonPropertyName("write")]
        [JsonProperty(PropertyName = "write", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Write { get; set; }
    }
}