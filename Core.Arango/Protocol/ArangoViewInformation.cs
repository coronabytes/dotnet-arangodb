using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango View Information
    /// </summary>
    public class ArangoViewInformation
    {
        /// <summary>
        ///     The name of the View.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("globallyUniqueId")]
        [JsonProperty(PropertyName = "globallyUniqueId")]
        public string GloballyUniqueId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id")]
        public string Id{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}