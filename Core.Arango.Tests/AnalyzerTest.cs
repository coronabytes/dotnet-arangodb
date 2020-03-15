using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class AnalyzerTest : TestBase
    {
        [Fact]
        public async Task Analyzers()
        {
            var analyzers = await Arango.ListAnalyzersAsync("test");

            await Arango.CreateAnalyzerAsync("test", new ArangoAnalyzer
            {
                Name = "text_de_nostem",
                Type = "text",
                Properties = new ArangoAnalyzerProperties
                {
                    Locale = "de.utf-8",
                    Case = "lower",
                    Accent = false,
                    Stopwords = new List<string>(),
                    Stemming = false
                },
                Features = new List<string> {"position", "norm", "frequency"}
            });

            await Arango.DeleteAnalyzerAsync("test", "text_de_nostem");
        }
    }
}