using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Base response model
    /// </summary>
    public abstract class ArangoResponseBase
    {
        /// <summary>
        ///     HTTP Status Code
        /// </summary>
        [JsonPropertyName("code")]
        [JsonProperty(PropertyName = "code")]
        public short Code { get; set; }

        /// <summary>
        ///     Has Error?
        /// </summary>
        [JsonPropertyName("error")]
        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        /// <summary>
        ///     Error Message
        /// </summary>
        [JsonPropertyName("errorMessage")]
        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     Error Code
        /// </summary>
        [JsonPropertyName("errorNum")]
        [JsonProperty(PropertyName = "errorNum")]
        public int ErrorNum { get; set; }
    }
}