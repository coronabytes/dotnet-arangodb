using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoFunctionDefinition
    {
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        [JsonProperty(PropertyName = "code", Required = Required.Always)]
        public string Code { get; set; }

        [JsonPropertyName("isDeterministic")]
        [JsonProperty(PropertyName = "isDeterministic", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsDeterministic { get; set; }
    }
}