using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoCollectionUpdate
    {
        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WaitForSync { get; set; }

        [JsonPropertyName("journalSize")]
        [JsonProperty(PropertyName = "journalSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? JournalSize { get; set; }

        /// <summary>
        ///  When updating collection this needs to contain existing schema or schema will be removed
        /// </summary>
        [JsonPropertyName("schema")]
        [JsonProperty(PropertyName = "schema", NullValueHandling = NullValueHandling.Include)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ArangoSchema Schema { get; set; }
    }
}