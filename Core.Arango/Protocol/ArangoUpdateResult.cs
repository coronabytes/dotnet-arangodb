using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///  Arango document update result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArangoUpdateResult<T>
    {
        /// <summary>
        ///  Id
        /// </summary>
        [JsonPropertyName("_id")]
        [JsonProperty(PropertyName = "_id")]
        public string Id { get; set; }

        /// <summary>
        ///  Key
        /// </summary>
        [JsonPropertyName("_key")]
        [JsonProperty(PropertyName = "_key")]
        public string Key { get; set; }

        /// <summary>
        ///   Revision
        /// </summary>
        [JsonPropertyName("_rev")]
        [JsonProperty(PropertyName = "_rev")]
        public string Revision { get; set; }

        /// <summary>
        ///  Old Revision
        /// </summary>
        [JsonPropertyName("_oldRev")]
        [JsonProperty(PropertyName = "_oldRev")]
        public string OldRevision { get; set; }

        /// <summary>
        ///  Old document
        /// </summary>
        [JsonPropertyName("old")]
        [JsonProperty(PropertyName = "old")]
        public T Old { get; set; }

        /// <summary>
        ///  New document
        /// </summary>
        [JsonPropertyName("new")]
        [JsonProperty(PropertyName = "new")]
        public T New { get; set; }
    }
}