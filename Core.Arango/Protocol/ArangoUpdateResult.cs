using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class ArangoUpdateResult<T>
    {
        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        [JsonPropertyName("_key")]
        [JsonProperty(PropertyName = "_key")]
        public string Key { get; set; }

        [JsonPropertyName("_rev")]
        [JsonProperty(PropertyName = "_rev")]
        public string Revision { get; set; }

        [JsonPropertyName("_oldRev")]
        [JsonProperty(PropertyName = "_oldRev")]
        public string OldRevision { get; set; }

        [JsonPropertyName("old")]
        [JsonProperty(PropertyName = "old")]
        public T Old { get; set; }

        [JsonPropertyName("new")]
        [JsonProperty(PropertyName = "new")]
        public T New { get; set; }
    }
}