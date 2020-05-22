using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.DevExtreme.Tests
{
    public class TransformTest
    {
        public TransformTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private readonly ITestOutputHelper _output;

        private DataSourceLoadOptionsBase DxLoad(Func<string, string> valueSource)
        {
            var loadOptions = new DataSourceLoadOptionsBase();
            DataSourceLoadOptionsParser.Parse(loadOptions, valueSource);
            return loadOptions;
        }

        [Fact]
        public void BoolTypeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = JArray.Parse(@"[[""name"",""=="",true],[""name"",""<>"", false]]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }

        [Fact]
        public void DateTimeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = JArray.Parse(
                    @"[[],""and"",[[""start"","">="",""2020-03-10T23:00:00.000Z""],""and"",[""start"",""<"",""2020-03-11T23:00:00.000Z""]]]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }

        [Fact]
        public void GroupLookups()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Sort = new[]
                {
                    new SortingInfo
                    {
                        Selector = "start",
                        Desc = true
                    }
                },
                Group = new[]
                {
                    new GroupingInfo
                    {
                        Selector = "projectKey",
                        Desc = false,
                        IsExpanded = false
                    }
                },
                TotalSummary = new[]
                {
                    new SummaryInfo
                    {
                        Selector = "duration",
                        SummaryType = "sum"
                    },
                    new SummaryInfo
                    {
                        Selector = "revenue",
                        SummaryType = "sum"
                    }
                },
                GroupSummary = new[]
                {
                    new SummaryInfo
                    {
                        Selector = "duration",
                        SummaryType = "sum"
                    },
                    new SummaryInfo
                    {
                        Selector = "revenue",
                        SummaryType = "sum"
                    }
                }
            }, new ArangoTransformSettings
            {
                GroupLookups = new Dictionary<string, string>
                {
                    ["ProjectKey"] = "DOCUMENT(AProject, ProjectKey).Name",
                    ["UserKey"] = "DOCUMENT(AUser, UserKey).Name"
                }
            });

            at.Transform(out var error);

            _output.WriteLine(at.AggregateExpression);
        }

        [Fact]
        public void InArrayTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                RequireTotalCount = false,
                Filter = JArray.Parse(
                    @"[[""categoryKeys"",""in"",""d9d48fe3-03dc-e611-80dd-0050568a3ed2""],""or"",[""categoryKeys"",""in"",""ad22d4ec-03dc-e611-80dd-0050568a3ed2""]]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);

            _output.WriteLine(at.FilterExpression);
        }

        [Fact]
        public void InArrayTest2()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                RequireTotalCount = false,
                Filter = JArray.Parse(
                    @"[[""categoryKeys"",""in"",""d9d48fe3-03dc-e611-80dd-0050568a3ed2""],""or"",[""categoryKeys"",""in"",""ad22d4ec-03dc-e611-80dd-0050568a3ed2""]]")
            }, new ArangoTransformSettings
            {
                ExtractFilters = new Dictionary<string, ArangoFilterTransform>
                {
                    ["CategoryKeys"] = new ArangoFilterTransform
                    {
                        IteratorVar = "z",
                        Collection = "Project",
                        Property = "CategoryKeys"
                    }
                }
            });

            at.Transform(out var error);

            _output.WriteLine(at.FilterExpression);
        }

        [Fact]
        public void NegateExpression()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                RequireTotalCount = true,
                Sort = new[]
                {
                    new SortingInfo
                    {
                        Selector = "start",
                        Desc = true
                    }
                },
                Filter = JArray.Parse(@"[[], ""and"", [""!"", [""scope"", ""=="", ""plan""]]]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);

            _output.WriteLine(at.FilterExpression);
        }

        [Fact]
        public void NegateExpression2()
        {
            var loadOptions = DxLoad(key =>
            {
                if (key == "filter")
                    return WebUtility.UrlDecode(
                        @"%5B%22!%22,%5B%5B%22type%22,%22=%22,null%5D,%22or%22,%5B%22type%22,%22=%22,1%5D%5D%5D");
                return null;
            });

            var at = new ArangoTransform(loadOptions, new ArangoTransformSettings());

            at.Transform(out var error);

            _output.WriteLine(at.FilterExpression);
        }

        [Fact]
        public void NegateExpression3()
        {
            var loadOptions = DxLoad(key =>
            {
                var r = key switch
                {
                    "take" => "100",
                    "requireTotalCount" => "true",
                    "sort" => "%5B%7B%22selector%22:%22start%22,%22desc%22:true%7D%5D",
                    "filter" => "%5B%5B%5D,%22and%22,%5B%22!%22,%5B%5B%22userKey%22,%22=%22,%2255770a6d-4dd7-42cf-9db5-aaff00d106d7%22%5D,%22or%22,%5B%22userKey%22,%22=%22,%2240ecb6b1-26cd-44e0-88df-a9457f4ade9c%22%5D%5D%5D%5D",
                    "totalSummary" => "%5B%7B%22selector%22:%22duration%22,%22summaryType%22:%22sum%22%7D,%7B%22selector%22:%22revenue%22,%22summaryType%22:%22sum%22%7D%5D",
                    _ => null
                };

                return key != null ? WebUtility.UrlDecode(r) : null;
            });

            var at = new ArangoTransform(loadOptions, new ArangoTransformSettings
            {
                ExtractFilters = new Dictionary<string, ArangoFilterTransform>
                {
                    ["UserKey"] = new ArangoFilterTransform
                    {
                        IteratorVar = "z",
                        Collection = "AUser",
                        Property = "_key"
                    }
                }
            });

            at.Transform(out var error);

            _output.WriteLine(at.FilterExpression);
        }

        // JArray.Parse differs from AspNetCore
        [Fact]
        public void NullTypeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = JArray.Parse(@"[""parentKey"",""="",null]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }


        [Fact]
        public void NumberTypeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = JArray.Parse(@"[[""duration"","">"",8],""and"",[""duration"","">"",8.5]]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }


        // JArray.Parse differs from AspNetCore
        [Fact]
        public void String2TypeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = JArray.Parse(@"[""data.invoiceNumber"",""contains"",""123""]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }

        [Fact]
        public void StringNumberTypeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = new List<string>
                {
                    "data.invoiceNumber", "startswith", "1234"
                }
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }

        [Fact]
        public void StringTypeTest()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                Filter = JArray.Parse(@"[[""name"",""contains"",""bad""],[""name"",""contains"",""""]]")
            }, new ArangoTransformSettings());

            at.Transform(out var error);
            var parameter = at.Parameter
                .Select(x => $"{x.Key}: {x.Value} [{x.Value?.GetType()}]")
                .ToList();

            _output.WriteLine(at.FilterExpression);
            _output.WriteLine(JsonConvert.SerializeObject(parameter, Formatting.Indented));
        }
    }
}