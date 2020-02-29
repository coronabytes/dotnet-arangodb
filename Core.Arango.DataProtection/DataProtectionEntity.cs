using Newtonsoft.Json;

namespace Core.Arango.DataProtection
{
    internal class DataProtectionEntity
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Key { get; set; }

        public string FriendlyName { get; set; }

        public string Xml { get; set; }
    }
}