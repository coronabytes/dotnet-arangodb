using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class FunctionCreateResponse : ArangoResponseBase
    {
        [JsonProperty(PropertyName = "isNewlyCreated")]
        public bool IsNewlyCreated { get; set; }
    }
}