using System.ComponentModel;
using Newtonsoft.Json;

namespace Core.Arango.DevExtreme
{
    public class DxLoadResult
    {
        public object Data
        {
            get
            {
                if (DataRaw != null)
                    return JsonConvert.DeserializeObject(DataRaw);

                return DataObj;
            }
        }

        [JsonIgnore] public string DataRaw { get; set; }
        [JsonIgnore] public object DataObj { get; set; }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int TotalCount { get; set; } = -1;

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int GroupCount { get; set; } = -1;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public decimal?[] Summary { get; set; }
    }
}