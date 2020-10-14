using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class FunctionRemoveRequest
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "group", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Group { get; set; }

        public static implicit operator FunctionRemoveRequest(string name) => new FunctionRemoveRequest { Name = name };
    }
}