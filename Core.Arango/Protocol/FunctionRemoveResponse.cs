using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    public class FunctionRemoveResponse : ArangoResponseBase
    {
        [JsonProperty(PropertyName = "deletedCount")]
        public int DeletedCount { get; set; }
    }
}