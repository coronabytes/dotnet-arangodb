using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoIndex
    {
        [JsonPropertyName("id")]
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty(PropertyName = "type")] 
        public ArangoIndexType Type { get; set; }

        [JsonPropertyName("fields")]
        [JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Fields { get; set; }

        [JsonPropertyName("minLength")]
        [JsonProperty(PropertyName = "minLength", NullValueHandling = NullValueHandling.Ignore)]
        public int? MinLength { get; set; }

        [JsonPropertyName("geoJson")]
        [JsonProperty(PropertyName = "geoJson", NullValueHandling = NullValueHandling.Ignore)]
        public bool? GeoJson { get; set; }

        [JsonPropertyName("sparse")]
        [JsonProperty(PropertyName = "sparse", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Sparse { get; set; }

        [JsonPropertyName("unique")]
        [JsonProperty(PropertyName = "unique", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Unique { get; set; }

        [JsonPropertyName("inBackground")]
        [JsonProperty(PropertyName = "inBackground", NullValueHandling = NullValueHandling.Ignore)]
        public bool? InBackground { get; set; }

        [JsonPropertyName("deduplicate")]
        [JsonProperty(PropertyName = "deduplicate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Deduplicate { get; set; }

        [JsonPropertyName("expireAfter")]
        [JsonProperty(PropertyName = "expireAfter", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpireAfter { get; set; }
    }
}