using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    ///   Arango Graph Edge Response
    /// </summary>
    public class ArangoEdgeResponse<T> : ArangoResponseBase
    {
        /// <summary>
        ///  Edge
        /// </summary>
        [JsonPropertyName("edge")]
        [JsonProperty(PropertyName = "edge")]
        public T Edge { get; set; }

        /// <summary>
        ///  New version
        /// </summary>
        [JsonPropertyName("new")]
        [JsonProperty(PropertyName = "new")]
        public T New { get; set; }

        /// <summary>
        ///  Old version
        /// </summary>
        [JsonPropertyName("old")]
        [JsonProperty(PropertyName = "old")]
        public T Old { get; set; }

        /// <summary>
        ///  When Removed
        /// </summary>
        [JsonPropertyName("removed")]
        [JsonProperty(PropertyName = "removed")]
        public bool Removed { get; set; }
    }
}