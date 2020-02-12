using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Xunit;

namespace Core.Arango.Tests
{
    public class AnalyzerTest : TestBase
    {
        [Fact]
        public async Task Analyzers()
        {
            await Arango.RefreshJwtAuth();

            await Arango.CreateDatabaseAsync("analyzers");

            var analyzers = await Arango.ListAnalyzersAsync("analyzers");

            await Arango.CreateAnalyzerAsync("analyzers", new ArangoAnalyzer
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
                Features = new List<string> { "position", "norm", "frequency" }
            });

            await Arango.DeleteAnalyzerAsync("analyzers", "text_de_nostem");
        }
    }
}
