using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango View Link Property
    /// </summary>
    public class ArangoLinkProperty
    {
        /// <summary>
        ///     A list of Analyzers, by name as defined via the Analyzers, that should be applied to values of processed document
        ///     attributes.
        ///     default: [ "identity" ]
        /// </summary>
        [JsonPropertyName("analyzers")]
        [JsonProperty(PropertyName = "analyzers", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<string> Analyzers { get; set; }

        /// <summary>
        ///     If set to true, then process all document attributes. Otherwise, only consider attributes mentioned in fields.
        ///     Attributes not explicitly specified in fields will be processed with default link properties, i.e. {}.
        /// </summary>
        [JsonPropertyName("includeAllFields")]
        [JsonProperty(PropertyName = "includeAllFields", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IncludeAllFields { get; set; }

        /// <summary>
        ///     If set to true, then for array values track the value position in arrays.
        /// </summary>
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

        /// <summary>
        ///     An object of fields that should be processed at each level of the document.
        ///     Each key specifies the document attribute to be processed.
        ///     Note that the value of includeAllFields is also consulted when selecting fields to be processed.
        ///     It is a recursive data structure.
        /// </summary>
        [JsonPropertyName("fields")]
        [JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, ArangoLinkProperty> Fields { get; set; }
    }
}