using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoLinkProperty
    {
        [JsonProperty(PropertyName = "analyzers", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> Analyzers { get; set; }

        [JsonProperty(PropertyName = "includeAllFields", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IncludeAllFields { get; set; }

        [JsonProperty(PropertyName = "trackListPositions", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TrackListPositions { get; set; }

        /// <summary>
        ///     none: Do not store values with the view.
        ///     id: Store information about value presence to allow use of the EXISTS() function
        /// </summary>
        [JsonProperty(PropertyName = "storeValues", NullValueHandling = NullValueHandling.Ignore)]
        public string StoreValues { get; set; }

        [JsonProperty(PropertyName = "fields", NullValueHandling = NullValueHandling.Ignore)]
        public IDictionary<string, ArangoLinkProperty> Fields { get; set; }
    }
}