using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class FunctionListRequest
    {
        [JsonProperty(PropertyName = "namespace")]
        public string Namespace { get; set; }

        public static implicit operator FunctionListRequest(string @namespace) => new FunctionListRequest { Namespace = @namespace };
    }
}