using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoIndex
    {
        [JsonProperty(PropertyName = "type")] public string Type { get; set; }

        [JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Fields { get; set; }

        [JsonProperty(PropertyName = "minLength", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength { get; set; }

        [JsonProperty(PropertyName = "geoJson", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GeoJson { get; set; }

        [JsonProperty(PropertyName = "sparse", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Sparse { get; set; }

        [JsonProperty(PropertyName = "unique", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Unique { get; set; }

        [JsonProperty(PropertyName = "inBackground", NullValueHandling = NullValueHandling.Ignore)]
        public bool? InBackground { get; set; }

        [JsonProperty(PropertyName = "deduplicate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Deduplicate { get; set; }

        [JsonProperty(PropertyName = "expireAfter", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpireAfter { get; set; }
    }
}