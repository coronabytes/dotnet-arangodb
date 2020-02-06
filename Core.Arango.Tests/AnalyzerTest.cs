using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Xunit;

namespace Core.Arango.Tests
{
    public class AnalyzerTest
    {
        [Fact]
        public async Task Analyzers()
        {
            var arango = new ArangoContext("Server=http://localhost:8529;Realm=unittest;User ID=root;Password=;");
            await arango.RefreshJwtAuth();

            await arango.CreateDatabaseAsync("analyzers");

            var analyzers = await arango.ListAnalyzersAsync("analyzers");

            await arango.CreateAnalyzerAsync("analyzers", new ArangoAnalyzer
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

            await arango.DeleteAnalyzerAsync("analyzers", "text_de_nostem");
        }
    }
}
