using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json.Linq;

namespace Core.Arango.DevExtreme
{
    public class ArangoTransform
    {
        private readonly DataSourceLoadOptionsBase _loadOption;
        private readonly ArangoTransformSettings _settings;
        private bool _isTransformed;

        public ArangoTransform(DataSourceLoadOptionsBase loadOption, ArangoTransformSettings settings)
        {
            _loadOption = loadOption;
            _settings = settings;
            HasGrouping = loadOption.Group?.Any() == true;

            if (loadOption.Take <= 0)
                loadOption.Take = 20;
            if (loadOption.Take > 1000)
                loadOption.Take = 1000;
        }

        public bool HasGrouping { get; }
        public Dictionary<string, object> Parameter { get; } = new Dictionary<string, object>();
        public string FilterExpression { get; private set; }
        public string SortExpression { get; private set; }
        public string AggregateExpression { get; private set; }

        public int Skip { get; private set; }
        public int Take { get; private set; }

        public List<string> Groups { get; } = new List<string>();
        public List<string> Summaries { get; } = new List<string>();

        public async Task<DxLoadResult> ExecuteAsync<T>(ArangoContext arango, ArangoHandle handle, string collection) where T : new()
        {
            if (!_isTransformed)
                throw new Exception("call transform first");

            var queryBuilder = new StringBuilder();
            queryBuilder.AppendLine($"FOR {_settings.IteratorVar} IN {collection}");
            queryBuilder.AppendLine("FILTER " + FilterExpression);

            if (_settings.Filter != null)
                queryBuilder.AppendLine(" && " + _settings.Filter);

            if (HasGrouping)
                queryBuilder.AppendLine(AggregateExpression);
            else
            {
                Parameter.TryAdd("SKIP", Skip);
                Parameter.TryAdd("TAKE", Take);

                queryBuilder.AppendLine(SortExpression);
                queryBuilder.AppendLine("LIMIT @SKIP, @TAKE");
                queryBuilder.AppendLine($"RETURN {_settings.Projection ?? _settings.IteratorVar}");
            }

            var query = queryBuilder.ToString();

            

            if (HasGrouping)
            {
                var res = await arango.QueryAsync<JObject>(handle, query, Parameter);

                return new DxLoadResult
                {
                    Data = BuildGrouping(this, res)
                };
            }
            else
            {
                var res = await arango.QueryAsync<T>(handle, query, Parameter, fullCount: _loadOption.RequireTotalCount);

                decimal?[] summary = null;

                if (_loadOption.TotalSummary?.Any() == true)
                {
                    var summaryQueryBuilder = new StringBuilder();
                    summaryQueryBuilder.AppendLine($"FOR {_settings.IteratorVar} IN {collection}");
                    summaryQueryBuilder.AppendLine("FILTER " + FilterExpression);

                    if (_settings.Filter != null)
                        summaryQueryBuilder.AppendLine(" && " + _settings.Filter);
                    summaryQueryBuilder.AppendLine(AggregateExpression);

                    // TODO: refactor
                    Parameter.Remove("SKIP");
                    Parameter.Remove("TAKE");

                    var summaryQuery = summaryQueryBuilder.ToString();

                    var summaryResult = await arango.QueryAsync<JObject>(handle, summaryQuery, Parameter);

                    summary = summaryResult.SingleOrDefault()?.PropertyValues()
                        .Select(x=>x.Value<decimal?>()).Skip(1).ToArray();
                }
                
                return new DxLoadResult
                {
                    Data = res,
                    Summary = summary,
                    TotalCount = res.FullCount ?? 0
                };
            }
        }

        public bool Transform(out string error)
        {
            if (_isTransformed)
                throw new Exception("already transformed");

            // TODO: Recursive
            if (_loadOption.Filter?.Count > 50)
            {
                error = "max filters 50 exceeded";
                return false;
            }

            if (_loadOption.Sort?.Length > 5)
            {
                error = "max sort levels of 5 exceeded";
                return false;
            }

            if (_loadOption.TotalSummary?.Length > 5)
            {
                error = "max total summaries of 5 exceeded";
                return false;
            }

            if (_loadOption.GroupSummary?.Length > 5)
            {
                error = "max group summaries of 5 exceeded";
                return false;
            }

            if (HasGrouping)
            {
                if (_loadOption.Group.Length > 5)
                {
                    error = "max grouping levels of 5 exceeded";
                    return false;
                }

                if (_loadOption.Group.Any(x => string.IsNullOrWhiteSpace(x.Selector)))
                {
                    error = "null group selector";
                    return false;
                }

                if (_loadOption.Group.Any(x => x.GroupInterval != null
                                               && x.GroupInterval != "year"
                                               && x.GroupInterval != "month"
                                               && x.GroupInterval != "day"))
                {
                    error = "invalid group interval";
                    return false;
                }

                if (_settings.RestrictGroups != null)
                    if (_loadOption.Group.Any(x =>
                        !_settings.RestrictGroups.Contains(x.Selector.FirstCharOfPropertiesToUpper())))
                    {
                        error = "restriced group selector";
                        return false;
                    }
            }

            error = null;

            FilterExpression = Filter();
            SortExpression = Sort();
            Skip = _loadOption.Skip;
            Take = _loadOption.Take;
            AggregateExpression = Aggregate();

            _isTransformed = true;

            return true;
        }

        private static IList GetRootFilter(IList loadOptionsFilter)
        {
            if (loadOptionsFilter != null && loadOptionsFilter.Count == 1)
                return GetRootFilter((JArray) loadOptionsFilter[0]);

            return loadOptionsFilter;
        }

        private string CreateParameter(object value)
        {
            var p = $"P{Parameter.Count + 1}";
            Parameter.Add(p, value);
            return $"@{p}";
        }

        private string PropertyName(string name)
        {
            if (name.Equals(_settings.Key, StringComparison.InvariantCultureIgnoreCase))
                name = "_key";

            return $"{_settings.IteratorVar}.{name}";
        }

       private string Aggregate()
        {
            if (_loadOption.RequireTotalCount || _loadOption.TotalSummary?.Any() == true ||
                _loadOption.Group?.Any() == true)
            {
                var sb = new StringBuilder();

                // GROUP

                sb.AppendLine("COLLECT");

                if (_loadOption.Group?.Any() == true)
                {
                    var groups = _loadOption.Group.Where(x => x.GroupInterval != "hour" && x.GroupInterval != "minute").Select(g =>
                    {
                        var selectorRight = g.Selector.FirstCharOfPropertiesToUpper();
                        var selectorLeft = g.Selector.FirstCharOfPropertiesToUpper().Replace(".","");

                        if (g.GroupInterval == "year")
                        {
                            Groups.Add($"YEAR{selectorLeft}");
                            return $"YEAR{selectorLeft} = DATE_YEAR({_settings.IteratorVar}.{selectorRight})";
                        }

                        if (g.GroupInterval == "month")
                        {
                            Groups.Add($"MONTH{selectorLeft}");
                            return $"MONTH{selectorLeft} = DATE_MONTH({_settings.IteratorVar}.{selectorRight})";
                        }

                        if (g.GroupInterval == "day")
                        {
                            Groups.Add($"DAY{selectorLeft}");
                            return $"DAY{selectorLeft} = DATE_DAY({_settings.IteratorVar}.{selectorRight})";
                        }

                        Groups.Add(selectorLeft);
                        return $"{selectorLeft} = {_settings.IteratorVar}.{selectorRight}";
                    }).ToList();

                    sb.AppendLine(string.Join(", ", groups));
                }

                // AGGREGATE

                var aggregates = new List<string>
                {
                    "TotalCount = LENGTH(1)"
                };

                if (_loadOption.Group?.Any() == true && _loadOption.GroupSummary?.Any() == true)
                {
                    aggregates.AddRange(_loadOption.GroupSummary.Select(s =>
                    {
                        var rightSelector = s.Selector.FirstCharOfPropertiesToUpper();
                        var leftSelector = s.Selector.FirstCharOfPropertiesToUpper().Replace(".","");
                        var op = s.SummaryType.ToUpperInvariant();

                        Summaries.Add($"{op}{leftSelector}");

                        if (op == "SUM" || op == "AVG" || op == "MIN" || op == "MAX" || op == "COUNT")
                            return $"{op}{leftSelector} = {op}({_settings.IteratorVar}.{rightSelector})";
                        return $"{op}{leftSelector} = SUM(0)";
                    }));
                }
                else if (_loadOption.TotalSummary?.Any() == true)
                {
                    aggregates.AddRange(_loadOption.TotalSummary.Select(s =>
                    {
                        var rightSelector = s.Selector.FirstCharOfPropertiesToUpper();
                        var leftSelector = s.Selector.FirstCharOfPropertiesToUpper().Replace(".","");
                        var op = s.SummaryType.ToUpperInvariant();

                        Summaries.Add($"{op}{leftSelector}");

                        if (op == "SUM" || op == "AVG" || op == "MIN" || op == "MAX" || op == "COUNT")
                            return $"{op}{leftSelector} = {op}({_settings.IteratorVar}.{rightSelector})";
                        return $"{op}{leftSelector} = SUM(0)";
                    }));
                }
                

                sb.AppendLine("AGGREGATE");
                sb.AppendLine(string.Join(", ", aggregates));

                // PROJECT TotalCount, Group Keys, Summaries

                var projection = new List<string> {"TotalCount"};

                foreach (var group in Groups)
                    projection.Add(group);

                foreach (var summary in Summaries)
                    projection.Add(summary);

                if (_loadOption.Group?.Any() == true)
                    sb.AppendLine(SortExpression.Replace(".","")); // TODO@AJ: Replace besser machen

                sb.AppendLine("RETURN {");
                sb.AppendLine(string.Join(", ", projection));
                sb.AppendLine("}");

                return sb.ToString();
            }

            return string.Empty;
        }

        private string Sort()
        {
            var sortingInfos = new List<SortingInfo>();

            if (_loadOption.Group?.Any() == true)
            {
                var groups = _loadOption.Group.Select(x => x)
                    .Where(x => x.GroupInterval != "hour" && x.GroupInterval != "minute").ToList();

                return "SORT " + string.Join(", ",
                           groups.Select(x =>
                           {
                               var prop = x.Selector.FirstCharOfPropertiesToUpper();

                               if (!string.IsNullOrWhiteSpace(x.GroupInterval))
                                   prop = x.GroupInterval.ToUpperInvariant() + prop;

                               return $"{prop} {(x.Desc ? "DESC" : "ASC")}";
                           }));
            }

            if (_loadOption.Sort != null)
                sortingInfos.AddRange(_loadOption.Sort.Where(x => x.Selector != null).ToList());
            else
                return string.Empty;


            return "SORT " + string.Join(", ",
                       sortingInfos.Select(x =>
                       {
                           var prop = PropertyName(x.Selector.FirstCharOfPropertiesToUpper());
                           return $"{prop} {(x.Desc ? "DESC" : "ASC")}";
                       }));
        }

        private string GetMatchingFilter(IList dxFilter)
        {
            if (dxFilter == null)
                return "true";

            if (dxFilter.Count == 0)
                return "true";

            if (dxFilter.Count == 2)
            {
                dxFilter.Add(dxFilter[1]);

                if (dxFilter[0] is JArray)
                    dxFilter[1] = JToken.FromObject("and");
                else
                    dxFilter[1] = JToken.FromObject("=");
            }

            var op = dxFilter[1];

            string opString;
            var logical = false;
            switch (op.ToString())
            {
                case "and":
                    opString = "&&";
                    logical = true;
                    break;
                case "or":
                    opString = "||";
                    logical = true;
                    break;
                case "=":
                    opString = "==";
                    break;
                case "<>":
                    opString = "!=";
                    break;
                case ">=":
                    opString = ">=";
                    break;
                case "<=":
                    opString = "<=";
                    break;
                case "<":
                    opString = "<";
                    break;
                case ">":
                    opString = ">";
                    break;
                case "contains":
                    opString = "CONTAINS";
                    break;
                case "notcontains":
                    opString = "NOTCONTAINS";
                    break;
                case "startswith":
                    opString = "STARTWITH";
                    break;
                case "endswith":
                    opString = "ENDSWITH";
                    break;
                default:
                    throw new NotImplementedException("Operation Type not implemented: " + op);
            }

            if (logical)
            {
                var logicalResult = string.Empty;

                for (var i = 0; i < dxFilter.Count; i++)
                    if (i % 2 == 0)
                    {
                        logicalResult += GetMatchingFilter((JArray) dxFilter[i]);
                        if (i + 1 != dxFilter.Count) logicalResult += $" {opString} ";
                    }


                return $"({logicalResult})";
            }

            var value = dxFilter[2]?.ToString();
            var property = PropertyName(dxFilter[0].ToString().FirstCharOfPropertiesToUpper());

            // UUID comps

            if (value == "null" || value == "" || value == null)
            {
                value = CreateParameter(null);
            }
            else if (Guid.TryParse(value, out var pGuid))
            {
                value = CreateParameter(pGuid == Guid.Empty ? null : pGuid.ToString("D"));
            }
            else if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var pDecimal))
            {
                value = CreateParameter(pDecimal);
            }
            else if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind,
                out var pDate))
            {
                value = CreateParameter(pDate);
            }
            else if (bool.TryParse(value, out var booleanValue))
            {
                value = CreateParameter(booleanValue);
            }

            else
            {
                var propertyCase = _loadOption.StringToLower ?? true ? $"LOWER({property})" : property;
                var valueyCase = _loadOption.StringToLower ?? true ? value.ToLowerInvariant() : value;

                switch (opString)
                {
                    case "CONTAINS":
                        return $@"{propertyCase} LIKE {CreateParameter($"%{valueyCase}%")}";
                    case "NOTCONTAINS":
                        return $@"{propertyCase} NOT LIKE {CreateParameter($"%{valueyCase}%")}";
                    case "STARTSWITH":
                        return $@"{propertyCase} LIKE {CreateParameter($"{valueyCase}%")}";
                    case "ENDSWITH":
                        return $@"{propertyCase} LIKE {CreateParameter($"%{valueyCase}")}'";
                    default:
                        value = CreateParameter(valueyCase);
                        break;
                }
            }


            return $"{property} {opString} {value}";
        }

        private string Filter()
        {
            var collapsedFilter = GetRootFilter(_loadOption.Filter);
            return GetMatchingFilter(collapsedFilter);
        }
        
        public List<ArangoGroup> BuildGrouping(ArangoTransform aq, List<JObject> list,
            Func<string, bool> restrict = null, ArangoGroup parent = null, int level = 0)
        {
            if (level >= aq.Groups.Count)
                return null;

            var res = new List<ArangoGroup>();

            var key = aq.Groups[level];

            var items = list.GroupBy(x => x.Value<string>(key)).ToList();

            foreach (var item in items)
            {
                var a = new ArangoGroup
                {
                    key = item.Key
                };

                var sublist = item.ToList();
                a.items = BuildGrouping(aq, sublist, restrict, a, level + 1);

                if (a.items == null)
                {
                    var row = sublist.Single();
                    a.count = row.Value<int>("TotalCount");

                    a.summary = aq.Summaries.Select(x =>
                    {
                        try
                        {
                            if (restrict?.Invoke(x) == true)
                                return 0m;
                            return row.Value<decimal?>(x) ?? 0m;
                        }
                        catch (Exception)
                        {
                            return (decimal?) 0m;
                        }
                    }).ToArray();
                }
                else
                {
                    a.count = null;

                    a.summary = aq.Summaries.Select((x, idx) =>
                    {
                        try
                        {
                            if (x.StartsWith("SUM") || x.StartsWith("COUNT"))
                                return a.items.Sum(y => y.summary[idx] ?? 0m);
                            if (x.StartsWith("MAX"))
                                return a.items.Max(y => y.summary[idx] ?? 0m);
                            if (x.StartsWith("MIN"))
                                return a.items.Min(y => y.summary[idx] ?? 0m);
                            if (x.StartsWith("AVG"))
                                return a.items.Average(y => y.summary[idx] ?? 0m); // TODO: Weight?

                            return 0m;
                        }
                        catch (Exception)
                        {
                            return (decimal?) 0m;
                        }
                    }).ToArray();
                }

                res.Add(a);
            }

            return res;
        }

        public class ArangoGroup
        {
            public string key { get; set; }
            public string displayName { get; set; }
            public List<ArangoGroup> items { get; set; }
            public int? count { get; set; }
            public decimal?[] summary { get; set; }
        }
    }
}