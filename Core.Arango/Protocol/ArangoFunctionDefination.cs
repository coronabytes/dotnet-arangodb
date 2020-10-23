using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoFunctionDefinition
    {
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "code", Required = Required.Always)]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "isDeterministic", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsDeterministic { get; set; }
    }
}