using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

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

        [JsonPropertyName("schema")]
        [JsonProperty(PropertyName = "schema", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public JSchema Schema { get; set; }
    }
}