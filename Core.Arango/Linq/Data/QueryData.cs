using System.Collections.Generic;
using Core.Arango.Linq.Attributes;
using Core.Arango.Linq.Interface;
using Newtonsoft.Json;

namespace Core.Arango.Linq.Data
{
    [CollectionProperty(Naming = NamingConvention.ToCamelCase)]
    internal class QueryData
    {
        public QueryData()
        {
            BindVars = new Dictionary<string, object>();
            Options = new QueryOption();
        }

        public string Query { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Count { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? BatchSize { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Ttl { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        //[JsonConverter(typeof(QueryParameterConverter))]
        public Dictionary<string, object> BindVars { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QueryOption Options { get; set; }
    }

    [CollectionProperty(Naming = NamingConvention.ToCamelCase)]
    public class QueryOption
    {
        public QueryOption()
        {
            Optimizer = new QueryOptimizerOption();
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? FullCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxPlans { get; set; }

        public QueryOptimizerOption Optimizer { get; set; }
    }

    [CollectionProperty(Naming = NamingConvention.ToCamelCase)]
    public class QueryOptimizerOption
    {
        public QueryOptimizerOption()
        {
            Rules = new List<string>();
        }

        public IList<string> Rules { get; set; }
    }

    public class QueryParameter
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}