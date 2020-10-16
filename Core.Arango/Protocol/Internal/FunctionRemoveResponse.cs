using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class FunctionRemoveResponse : ResponseBase
    {
        [JsonProperty(PropertyName = "deletedCount")]
        public int DeletedCount { get; set; }
    }
}