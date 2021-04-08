using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango collection key generating options
    /// </summary>
    public class ArangoKeyOptions
    {
        /// <summary>
        ///     Specifies the type of the key generator.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ArangoKeyType? Type { get; set; }

        /// <summary>
        ///     If set to true, then it is allowed to supply own key values in the _key attribute of a document.
        ///     If set to false, then the key generator will solely be responsible for generating keys and supplying own key values
        ///     in the _key attribute of documents is considered an error.
        /// </summary>
        [JsonPropertyName("allowUserKeys")]
        [JsonProperty(PropertyName = "allowUserKeys", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? AllowUserKeys { get; set; }

        /// <summary>
        ///     increment value for autoincrement key generator. Not used for other key generator types.
        /// </summary>
        [JsonPropertyName("increment")]
        [JsonProperty(PropertyName = "increment", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Increment { get; set; }

        /// <summary>
        ///     Initial offset value for autoincrement key generator. Not used for other key generator types.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonProperty(PropertyName = "offset", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? Offset { get; set; }
    }
}