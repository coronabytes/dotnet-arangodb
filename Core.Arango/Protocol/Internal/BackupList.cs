using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol.Internal
{
    internal class BackupList
    {
        [JsonPropertyName("server")]
        [JsonProperty(PropertyName = "server")]
        public string Server { get; set; }

        [JsonPropertyName("list")]
        [JsonProperty(PropertyName = "list")]
        public SortedDictionary<string, ArangoBackup> List { get; set; }
    }
}