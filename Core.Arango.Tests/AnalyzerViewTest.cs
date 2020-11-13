using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class AnalyzerViewTest : TestBase
    {
        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task AnalyzersViews(IArangoContext arango)
        {
            await SetupAsync(arango);
            var analyzers = await Arango.Analyzer.ListAsync("test");

            await Arango.Analyzer.CreateAsync("test", new ArangoAnalyzer
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


            await Arango.Collection.CreateAsync("test", "collection", ArangoCollectionType.Document);

            await Arango.View.CreateAsync("test", new ArangoView
            {
                Name = "TestView",
                Links = new Dictionary<string, ArangoLinkProperty>
                {
                    ["collection"] = new ArangoLinkProperty
                    {
                        Fields = new Dictionary<string, ArangoLinkProperty>
                        {
                            ["Name"] = new ArangoLinkProperty
                            {
                                Analyzers = new List<string> {"text_de_nostem"}
                            }
                        }
                    }
                }
            });

            await Arango.View.DropAllAsync("test");

            await Arango.Analyzer.DeleteAsync("test", "text_de_nostem", true);
        }
    }
}