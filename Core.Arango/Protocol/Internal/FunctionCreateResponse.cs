using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class FunctionCreateResponse : ResponseBase
    {
        [JsonProperty(PropertyName = "isNewlyCreated")]
        public bool IsNewlyCreated { get; set; }
    }
}