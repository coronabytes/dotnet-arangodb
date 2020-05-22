using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json.Linq;

namespace Core.Arango.DevExtreme
{
    internal enum TypeHint
    {
        Unsure,
        String,
        DateOrNumber
    }

    /// <summary>
    ///     DevExtreme DataSourceLoadOptions to AQL
    /// </summary>
    public class ArangoTransform
    {
        private readonly DataSourceLoadOptionsBase _loadOption;
        private readonly ArangoTransformSettings _settings;

        private readonly List<(string, string, bool, string)> _extractedFilters =
            new List<(string, string, bool, string)>();

        private bool _isTransformed;

        public ArangoTransform(DataSourceLoadOptionsBase loadOption, ArangoTransformSettings settings)
        {
            _loadOption = loadOption;
            _settings = settings;
            HasGrouping = loadOption.Group?.Any() == true;

            if (loadOption.Take <= 0)
                loadOption.Take = settings.DefaultTake;
            if (loadOption.Take > settings.MaxTake)
                loadOption.Take = settings.MaxTake;
        }

        public bool HasGrouping { get; }
        public Dictionary<string, object> Parameter { get; } = new Dictionary<string, object>();
        public string FilterExpression { get; private set; }
        public string SortExpression { get; private set; }
        public string AggregateExpression { get; private set; }


        public Dictionary<string, string> ExtractedFilters { get; } = new Dictionary<string, string>();

        public int Skip { get; private set; }
        public int Take { get; private set; }

        public List<string> Groups { get; } = new List<string>();
        public List<string> Summaries { get; } = new List<string>();

        /// <summary>
        ///     Executes transformed query asynchronous
        /// </summary>
        public async Task<DxLoadResult> ExecuteAsync<T>(ArangoContext arango,
            ArangoHandle handle,
            string collection,
            CancellationToken cancellationToken = default)
            where T : new()
        {
            if (!_isTransformed)
                throw new Exception("call transform first");

            var queryBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(_settings.Preamble))
                queryBuilder.AppendLine(_settings.Preamble);

            queryBuilder.AppendLine($"FOR {_settings.IteratorVar} IN {collection}");
            queryBuilder.AppendLine("FILTER " + FilterExpression);

            if (_settings.Filter != null)
                queryBuilder.AppendLine(" && " + _settings.Filter);

            if (HasGrouping)
            {
                queryBuilder.AppendLine(AggregateExpression);
            }
            else
            {
                Parameter.TryAdd("SKIP", Skip);
                Parameter.TryAdd("TAKE", Take);

                queryBuilder.AppendLine(SortExpression);
                queryBuilder.AppendLine("LIMIT @SKIP, @TAKE");

                if (!string.IsNullOrWhiteSpace(_settings.PreProjection))
                    queryBuilder.AppendLine(_settings.PreProjection);

                queryBuilder.AppendLine($"RETURN {_settings.Projection ?? _settings.IteratorVar}");
            }

            var query = queryBuilder.ToString();


            if (HasGrouping)
            {
                var res = await arango.QueryAsync<JObject>(handle, query, Parameter,
                    cancellationToken: cancellationToken);

                return new DxLoadResult
                {
                    Data = BuildGrouping(this, res)
                };
            }
            else
            {
                var res = await arango.QueryAsync<T>(handle, query, Parameter,
                    fullCount: _loadOption.RequireTotalCount, cancellationToken: cancellationToken);

                decimal?[] summary = null;

                if (_loadOption.TotalSummary?.Any() == true)
                {
                    var summaryQueryBuilder = new StringBuilder();

                    if (!string.IsNullOrWhiteSpace(_settings.Preamble))
                        summaryQueryBuilder.AppendLine(_settings.Preamble);

                    summaryQueryBuilder.AppendLine($"FOR {_settings.IteratorVar} IN {collection}");
                    summaryQueryBuilder.AppendLine("FILTER " + FilterExpression);

                    if (_settings.Filter != null)
                        summaryQueryBuilder.AppendLine(" && " + _settings.Filter);
                    summaryQueryBuilder.AppendLine(AggregateExpression);

                    // TODO: refactor
                    Parameter.Remove("SKIP");
                    Parameter.Remove("TAKE");

                    var summaryQuery = summaryQueryBuilder.ToString();

                    var summaryResult = await arango.QueryAsync<JObject>(handle, summaryQuery, Parameter,
                        cancellationToken: cancellationToken);

                    summary = summaryResult.SingleOrDefault()?.PropertyValues()
                        .Select(x => x.Value<decimal?>()).Skip(1).ToArray();
                }

                return new DxLoadResult
                {
                    Data = res,
                    Summary = summary,
                    TotalCount = res.FullCount ?? -1
                };
            }
        }

        public bool Transform(out string error)
        {
            if (_isTransformed)
                throw new Exception("already transformed");

            try
            {
                // TODO: Recursive
                if (_loadOption.Filter?.Count > _settings.MaxFilter)
                {
                    error = $"max filters {_settings.MaxFilter} exceeded";
                    return false;
                }

                if (_loadOption.Sort?.Length > _settings.MaxSort)
                {
                    error = $"max sort levels of {_settings.MaxSort} exceeded";
                    return false;
                }

                if (_loadOption.TotalSummary?.Length > _settings.MaxSummary)
                {
                    error = $"max total summaries of {_settings.MaxSummary} exceeded";
                    return false;
                }

                if (_loadOption.GroupSummary?.Length > _settings.MaxSummary)
                {
                    error = $"max group summaries of {_settings.MaxSummary} exceeded";
                    return false;
                }

                if (HasGrouping)
                {
                    if (_loadOption.Group.Length > _settings.MaxGroup)
                    {
                        error = $"max grouping levels of {_settings.MaxGroup} exceeded";
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
                        if (_loadOption.Group.Any(x => !_settings.RestrictGroups
                            .Contains(x.Selector.FirstCharOfPropertiesToUpper())))
                        {
                            error = "restricted group selector";
                            return false;
                        }
                }

                error = null;

                FilterExpression = Filter();



                foreach (var a in _extractedFilters.GroupBy(x => x.Item1))
                {
                    var subfilters = a.GroupBy(y => y.Item2)
                        .Select(y =>
                            // TODO: Split by not ? or ist this just impossible
                            (y.Any(w => w.Item3) ? "!" : "")
                            + "(" + string.Join(" || ", y.Select(z => z.Item4)) + ")");

                    ExtractedFilters[a.Key] = "(" + string.Join(" && ", subfilters) + ")";
                }


                SortExpression = Sort();
                Skip = _loadOption.Skip;
                Take = _loadOption.Take;
                AggregateExpression = Aggregate();

                _isTransformed = true;

                return true;
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }
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

        private string PropertyName(string name, string iteratorVar = null)
        {
            if (name.Equals(_settings.Key, StringComparison.InvariantCultureIgnoreCase))
                name = "_key";

            var nameLambda = _settings.PropertyTransform;

            if (nameLambda != null)
                return _settings.PropertyTransform(name, _settings);

            return $"{iteratorVar ?? _settings.IteratorVar}.{name}";
        }

        private string Aggregate()
        {
            if (_loadOption.RequireTotalCount || _loadOption.TotalSummary?.Any() == true ||
                _loadOption.Group?.Any() == true)
            {
                var sb = new StringBuilder();

                sb.AppendLine("COLLECT");

                if (_loadOption.Group?.Any() == true)
                {
                    var groups = _loadOption.Group.Where(x => x.GroupInterval != "hour" && x.GroupInterval != "minute")
                        .Select(g =>
                        {
                            var selector = _settings.ValidPropertyName(g.Selector).FirstCharOfPropertiesToUpper();

                            var selectorRight = selector;
                            var selectorLeft = selector.Replace(".", "");

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

                var aggregates = new List<string>
                {
                    "TotalCount = LENGTH(1)"
                };

                if (_loadOption.Group?.Any() == true && _loadOption.GroupSummary?.Any() == true)
                    aggregates.AddRange(_loadOption.GroupSummary.Select(s =>
                    {
                        var selector = _settings.ValidPropertyName(s.Selector).FirstCharOfPropertiesToUpper();

                        var rightSelector = selector;
                        var leftSelector = selector.Replace(".", "");
                        var op = s.SummaryType.ToUpperInvariant();

                        Summaries.Add($"{op}{leftSelector}");

                        if (op == "SUM" || op == "AVG" || op == "MIN" || op == "MAX" || op == "COUNT")
                            return $"{op}{leftSelector} = {op}({_settings.IteratorVar}.{rightSelector})";
                        return $"{op}{leftSelector} = SUM(0)";
                    }));
                else if (_loadOption.TotalSummary?.Any() == true)
                    aggregates.AddRange(_loadOption.TotalSummary.Select(s =>
                    {
                        var selector = _settings.ValidPropertyName(s.Selector).FirstCharOfPropertiesToUpper();
                        var rightSelector = selector;
                        var leftSelector = selector.Replace(".", "");
                        var op = s.SummaryType.ToUpperInvariant();

                        Summaries.Add($"{op}{leftSelector}");

                        if (op == "SUM" || op == "AVG" || op == "MIN" || op == "MAX" || op == "COUNT")
                            return $"{op}{leftSelector} = {op}({_settings.IteratorVar}.{rightSelector})";
                        return $"{op}{leftSelector} = SUM(0)";
                    }));


                sb.AppendLine("AGGREGATE");
                sb.AppendLine(string.Join(", ", aggregates));

                var projection = new List<string> {"TotalCount"};

                foreach (var group in Groups)
                    projection.Add(group);

                if (_settings.GroupLookups?.Any() == true)
                    foreach (var glookup in _settings.GroupLookups)
                    {
                        var g = Groups.SingleOrDefault(x =>
                            x.Equals(glookup.Key, StringComparison.InvariantCultureIgnoreCase));

                        if (g != null) projection.Add($"{g}_DV: {glookup.Value}");
                    }

                foreach (var summary in Summaries)
                    projection.Add(summary);

                if (_loadOption.Group?.Any() == true)
                    sb.AppendLine(SortExpression.Replace(".", "")); // Refactor

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
                        var prop = _settings.ValidPropertyName(x.Selector).FirstCharOfPropertiesToUpper();

                        if (!string.IsNullOrWhiteSpace(x.GroupInterval))
                            prop = x.GroupInterval.ToUpperInvariant() + prop;

                        return $"{prop} {(x.Desc ? "DESC" : "ASC")}";
                    }));
            }

            if (_loadOption.Sort != null)
                sortingInfos.AddRange(_loadOption.Sort.Where(x => x.Selector != null).ToList());
            else
                return _settings.StableSort ? $"SORT {_settings.IteratorVar}._key" : string.Empty;


            var sort = "SORT " + string.Join(", ",
                sortingInfos.Select(x =>
                {
                    var prop = PropertyName(_settings.ValidPropertyName(x.Selector)
                        .FirstCharOfPropertiesToUpper());
                    return $"{prop} {(x.Desc ? "DESC" : "ASC")}";
                }));

            if (_settings.StableSort && !sort.Contains("_key"))
                sort += $", {_settings.IteratorVar}._key";

            return sort;
        }

        private string GetMatchingFilter(IList dxFilter, bool not = false)
        {
            if (dxFilter == null)
                return "true";

            if (dxFilter.Count == 0)
                return "true";

            if (dxFilter.Count == 2)
            {
                if ((dxFilter[0] is JValue v && v.Value is string s && s == "!"
                     || dxFilter[0] is string s2 && s2 == "!") && dxFilter[1] is JArray)
                {
                    var r = GetMatchingFilter((JArray) dxFilter[1], true);
                    return $"!({r})";
                }

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
                case "==":
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
                    opString = "STARTSWITH";
                    break;
                case "endswith":
                    opString = "ENDSWITH";
                    break;
                case "in":
                    opString = "IN";
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
                        logicalResult += GetMatchingFilter((JArray) dxFilter[i], not);
                        if (i + 1 != dxFilter.Count) logicalResult += $" {opString} ";
                    }


                return $"({logicalResult})";
            }

            var rawValue = dxFilter[2];

            var realPropertyName = _settings.ValidPropertyName(dxFilter[0].ToString()).FirstCharOfPropertiesToUpper();

            string property;

            if (_settings.ExtractFilters.TryGetValue(realPropertyName, out var extract1))
                property = PropertyName(extract1.Property, extract1.IteratorVar);
            else
                property = PropertyName(realPropertyName);


            string boundParam = null;

            var returnValue = "";

            switch (rawValue)
            {
                case null:
                    boundParam = CreateParameter(null);
                    break;
                case bool b:
                    boundParam = CreateParameter(b);
                    break;
                case int i:
                    boundParam = CreateParameter(i);
                    break;
                case long l:
                    boundParam = CreateParameter(l);
                    break;
                case decimal m:
                    boundParam = CreateParameter(m);
                    break;
                case float f:
                    boundParam = CreateParameter(f);
                    break;
                case double d:
                    boundParam = CreateParameter(d);
                    break;
                case Guid guid:
                    boundParam = CreateParameter(guid);
                    break;
                case DateTime datetime:
                    boundParam = CreateParameter(datetime);
                    break;
                case string s:
                {
                    var propertyCase = _loadOption.StringToLower ?? true ? $"LOWER({property})" : property;
                    var valueCase = _loadOption.StringToLower ?? true ? s.ToLowerInvariant() : s;

                    switch (opString)
                    {
                        case "CONTAINS":
                            returnValue = $@"{propertyCase} LIKE {CreateParameter($"%{valueCase}%")}";
                            break;
                        case "NOTCONTAINS":
                            returnValue = $@"{propertyCase} NOT LIKE {CreateParameter($"%{valueCase}%")}";
                            break;
                        case "STARTSWITH":
                            returnValue = $@"{propertyCase} LIKE {CreateParameter($"{valueCase}%")}";
                            break;
                        case "ENDSWITH":
                            returnValue = $@"{propertyCase} LIKE {CreateParameter($"%{valueCase}")}'";
                            break;
                        default:
                            boundParam = CreateParameter(s);
                            break;
                    }

                    break;
                }
                case JValue jv:
                {
                    var type = jv.Type;

                    if (type == JTokenType.String /*|| typeHint == TypeHint.String*/)
                    {
                        var v = jv.Value as string ?? string.Empty; //?? jv.Value.ToString();

                        var propertyCase = _loadOption.StringToLower ?? true ? $"LOWER({property})" : property;
                        var valueCase = _loadOption.StringToLower ?? true ? v.ToLowerInvariant() : v;

                        switch (opString)
                        {
                            case "CONTAINS":
                                returnValue = $@"{propertyCase} LIKE {CreateParameter($"%{valueCase}%")}";
                                break;
                            case "NOTCONTAINS":
                                returnValue = $@"{propertyCase} NOT LIKE {CreateParameter($"%{valueCase}%")}";
                                break;
                            case "STARTSWITH":
                                returnValue = $@"{propertyCase} LIKE {CreateParameter($"{valueCase}%")}";
                                break;
                            case "ENDSWITH":
                                returnValue = $@"{propertyCase} LIKE {CreateParameter($"%{valueCase}")}'";
                                break;
                            default:
                                boundParam = CreateParameter(v);
                                break;
                        }
                    }
                    else
                    {
                        boundParam = CreateParameter(jv.Value);
                    }

                    break;
                }
                default:
                {
                    var type = rawValue.GetType();
                    throw new NotImplementedException($"Value of type {type}");
                }
            }


            if (string.IsNullOrWhiteSpace(returnValue))
            {
                if (opString == "IN")
                    returnValue = $"{boundParam} IN {property}";
                else
                    returnValue = $"{property} {opString} {boundParam}";
            }

            if (_settings.ExtractFilters.TryGetValue(realPropertyName, out var extract))
            {
                _extractedFilters.Add((extract.Collection, realPropertyName, not, returnValue));

                returnValue = not ? "false" : "true";
            }

            return returnValue;
        }

        private string Filter()
        {
            var collapsedFilter = GetRootFilter(_loadOption.Filter);
            var filter = GetMatchingFilter(collapsedFilter);
            return filter;
        }


        public List<DxGroupResult> BuildGrouping(ArangoTransform aq, List<JObject> list,
            Func<string, bool> restrict = null, DxGroupResult parent = null, int level = 0)
        {
            if (level >= aq.Groups.Count)
                return null;

            var res = new List<DxGroupResult>();

            var key = aq.Groups[level];

            var items = list.GroupBy(x => x.Value<string>(key)).ToList();

            foreach (var item in items)
            {
                var a = new DxGroupResult
                {
                    Key = item.Key
                };

                if (_settings.GroupLookups?.ContainsKey(key) == true)
                    a.Display = item.FirstOrDefault()?.Value<string>($"{key}_DV");

                var sublist = item.ToList();
                a.Items = BuildGrouping(aq, sublist, restrict, a, level + 1);

                if (a.Items == null)
                {
                    var row = sublist.Single();
                    a.Count = row.Value<int>("TotalCount");

                    a.Summary = aq.Summaries.Select(x =>
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
                    a.Count = null;

                    a.Summary = aq.Summaries.Select((x, idx) =>
                    {
                        try
                        {
                            if (x.StartsWith("SUM") || x.StartsWith("COUNT"))
                                return a.Items.Sum(y => y.Summary[idx] ?? 0m);
                            if (x.StartsWith("MAX"))
                                return a.Items.Max(y => y.Summary[idx] ?? 0m);
                            if (x.StartsWith("MIN"))
                                return a.Items.Min(y => y.Summary[idx] ?? 0m);
                            if (x.StartsWith("AVG"))
                                return a.Items.Average(y => y.Summary[idx] ?? 0m); // TODO: Weight?

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
    }
}