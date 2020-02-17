using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Arango.DevExtreme
{
    public class DxGroupResult
    {
        [JsonProperty("key")] public string Key { get; set; }

        [JsonProperty("items")] public List<DxGroupResult> Items { get; set; }

        [JsonProperty("count")] public int? Count { get; set; }

        [JsonProperty("summary")] public decimal?[] Summary { get; set; }
    }
}