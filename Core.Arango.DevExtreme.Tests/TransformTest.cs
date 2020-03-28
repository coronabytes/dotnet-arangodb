using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DevExtreme.AspNet.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.DevExtreme.Tests
{
    public class TransformTest
    {
        private readonly ITestOutputHelper _output;

        public TransformTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void NegateExpression()
        {
            var at = new ArangoTransform(new DataSourceLoadOptionsBase
            {
                Take = 20,
                RequireTotalCount = true,
                Sort = new []
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
                Filter = JArray.Parse(@"[[],""and"",[[""start"","">="",""2020-03-10T23:00:00.000Z""],""and"",[""start"",""<"",""2020-03-11T23:00:00.000Z""]]]")
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
    }
}
