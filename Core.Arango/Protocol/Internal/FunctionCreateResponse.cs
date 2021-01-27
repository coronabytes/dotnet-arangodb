using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class FunctionCreateResponse : ArangoResponseBase
    {
        [JsonProperty(PropertyName = "isNewlyCreated")]
        public bool IsNewlyCreated { get; set; }
    }
}