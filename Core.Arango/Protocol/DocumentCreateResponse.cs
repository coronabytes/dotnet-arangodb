using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    internal class DocumentCreateResponse<T>
    {
        [JsonProperty(PropertyName = "_id")] public string Id { get; set; }

        [JsonProperty(PropertyName = "_key")] public string Key { get; set; }

        [JsonProperty(PropertyName = "_rev")] public string Revision { get; set; }

        [JsonProperty(PropertyName = "new")] public T New { get; set; }

        [JsonProperty(PropertyName = "old")] public T Old { get; set; }

        [JsonProperty(PropertyName = "error")] public bool Error { get; set; }

        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }
    }
}