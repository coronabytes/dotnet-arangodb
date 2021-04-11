using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///     Arango Graph Description
    /// </summary>
    public class ArangoGraph
    {
        /// <summary>
        ///     Name of the graph.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     An array of definitions for the relations of the graph. Each has the following type:
        /// </summary>
        [JsonPropertyName("edgeDefinitions")]
        [JsonProperty(PropertyName = "edgeDefinitions")]
        public IList<ArangoEdgeDefinition> EdgeDefinitions { get; set; }

        /// <summary>
        ///     An array of additional vertex collections. Documents within these collections do not have edges within this graph.
        /// </summary>
        [JsonPropertyName("orphanCollections")]
        [JsonProperty(PropertyName = "orphanCollections", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IList<string> OrphanCollections { get; set; }

        /// <summary>
        ///     Define if the created graph should be smart (Enterprise Edition only).
        /// </summary>
        [JsonPropertyName("isSmart")]
        [JsonProperty(PropertyName = "isSmart", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsSmart { get; set; }

        /// <summary>
        ///     Whether to create a Disjoint SmartGraph instead of a regular SmartGraph (Enterprise Edition only).
        /// </summary>
        [JsonPropertyName("isDisjoint")]
        [JsonProperty(PropertyName = "isDisjoint", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsDisjoint { get; set; }

        /// <summary>
        ///     JSON object to define options for creating collections within this graph. It can contain the following attributes:
        /// </summary>
        [JsonPropertyName("options")]
        [JsonProperty(PropertyName = "options", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public ArangoGraphOptions Options { get; set; }

        /// <summary>
        ///  Overflow properties
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        [System.Text.Json.Serialization.JsonExtensionData]
        public Dictionary<string, object> ExtensionData { get; set; }
    }
}