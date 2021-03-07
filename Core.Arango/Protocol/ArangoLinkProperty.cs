using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoLinkProperty
    {
        /// <summary>
        ///   
        /// </summary>
        [JsonPropertyName("analyzers")]
        [JsonProperty(PropertyName = "analyzers", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IList<string> Analyzers { get; set; }

        [JsonPropertyName("includeAllFields")]
        [JsonProperty(PropertyName = "includeAllFields", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IncludeAllFields { get; set; }

        [JsonPropertyName("trackListPositions")]
        [JsonProperty(PropertyName = "trackListPositions", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? TrackListPositions { get; set; }

        /// <summary>
        ///     none: Do not store values with the view.
        ///     id: Store information about value presence to allow use of the EXISTS() function
        /// </summary>
        [JsonPropertyName("storeValues")]
        [JsonProperty(PropertyName = "storeValues", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string StoreValues { get; set; }

        [JsonPropertyName("fields")]
        [JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, ArangoLinkProperty> Fields { get; set; }
    }
}