using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango collections updatable properties
    /// </summary>
    public class ArangoCollectionUpdate
    {
        /// <summary>
        ///    If true then the data is synchronized to disk before returning from a document create, update, replace or removal operation. (default: false)
        /// </summary>
        [JsonPropertyName("waitForSync")]
        [JsonProperty(PropertyName = "waitForSync", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? WaitForSync { get; set; }

        // MMFiles
        /*[JsonPropertyName("journalSize")]
        [JsonProperty(PropertyName = "journalSize", NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public long? JournalSize { get; set; }*/

        /// <summary>
        ///    Optional object that specifies the collection level schema for documents.
        ///    When updating collection this needs to contain existing schema or schema will be removed
        /// </summary>
        [JsonPropertyName("schema")]
        [JsonProperty(PropertyName = "schema", NullValueHandling = NullValueHandling.Include)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        public ArangoSchema Schema { get; set; }
    }
}