using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Arango.Protocol
{
    /// <summary>
    /// Base response model
    /// </summary>
    public abstract class ArangoResponseBase
    {
        [JsonProperty(PropertyName = "code")]
        public short Code { get; set; }

        [JsonProperty(PropertyName = "error")]
        public bool Error { get; set; }

        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty(PropertyName = "errorNum")]
        public int ErrorNum { get; set; }
    }
}