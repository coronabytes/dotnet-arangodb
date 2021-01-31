using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class QueryResponseExtra
    {
        [JsonPropertyName("stats")]
        [JsonProperty(PropertyName = "stats")]
        public ArangoQueryStatistic Statistic { get; set; }
    }
}