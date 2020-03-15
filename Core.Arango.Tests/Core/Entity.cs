using Newtonsoft.Json;

namespace Core.Arango.Tests.Core
{
    public class Entity
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Key { get; set; }

        public int Value { get; set; }
    }
}