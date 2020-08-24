using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoUser
    {
        [JsonProperty(PropertyName = "user")] public string Name { get; set; }

        [JsonProperty(PropertyName = "passwd")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "active", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? Active { get; set; }

        [JsonProperty(PropertyName = "extra", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public object Extra { get; set; }
    }
}