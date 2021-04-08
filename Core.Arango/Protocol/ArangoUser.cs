using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango User
    /// </summary>
    public class ArangoUser
    {
        /// <summary>
        ///     The name of the user as a string. This is mandatory.
        /// </summary>
        [JsonPropertyName("user")]
        [JsonProperty(PropertyName = "user")]
        public string Name { get; set; }

        /// <summary>
        ///     The user password as a string. If no password is specified, the empty string will be used.
        /// </summary>
        [JsonPropertyName("passwd")]
        [JsonProperty(PropertyName = "passwd")]
        public string Password { get; set; }

        /// <summary>
        ///     An optional flag that specifies whether the user is active. If not specified, this will default to true
        /// </summary>
        [JsonPropertyName("active")]
        [JsonProperty(PropertyName = "active", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Active { get; set; }

        /// <summary>
        ///     An optional JSON object with arbitrary extra data about the user.
        /// </summary>
        [JsonPropertyName("extra")]
        [JsonProperty(PropertyName = "extra", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Extra { get; set; }
    }
}