using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
    /// 
    /// </summary>
    public class ArangoExplainResult : ArangoResponseBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("plan")]
        [JsonProperty(PropertyName = "plan")]
        public Dictionary<string, object> Plan { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("allPlans")]
        [JsonProperty(PropertyName = "allPlans")]
        public ICollection<Dictionary<string, object>> AllPlans { get; set; }
    }
}