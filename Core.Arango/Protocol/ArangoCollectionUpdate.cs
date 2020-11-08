using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Core.Arango.Protocol
{
    public class ArangoCollectionUpdate
    {
        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        public bool? WaitForSync { get; set; }

        [JsonPropertyName("journalSize")]
        [JsonProperty(PropertyName = "journalSize", NullValueHandling = NullValueHandling.Ignore)]
        public long? JournalSize { get; set; }

        [JsonPropertyName("schema")]
        [JsonProperty(PropertyName = "schema", NullValueHandling = NullValueHandling.Ignore)]
        public JSchema Schema { get; set; }
    }
}