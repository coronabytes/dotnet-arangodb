using DevExtreme.AspNet.Data;
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
    }
}
