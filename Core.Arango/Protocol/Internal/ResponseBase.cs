using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    /// <summary>
    /// Base response model
    /// </summary>
    internal abstract class ResponseBase
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