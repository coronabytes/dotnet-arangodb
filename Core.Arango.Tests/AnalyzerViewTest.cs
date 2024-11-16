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
        public async ValueTask AnalyzersViews(string serializer)
        {
            await SetupAsync(serializer);

            await Arango.Analyzer.CreateAsync("test", new ArangoAnalyzer
            {
                Name = "text_de_nostem",
                Type = ArangoAnalyzerType.Text,
                Properties = new ArangoAnalyzerProperties
                {
                    Locale = "de.utf-8",
                    Case = ArangoAnalyzerCase.Lower,
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
                    ["collection"] = new()
                    {
                        Fields = new Dictionary<string, ArangoLinkProperty>
                        {
                            ["Name"] = new()
                            {
                                Analyzers = new List<string> {"text_de_nostem"}
                            }
                        }
                    }
                }
            });


            var analyzers = await Arango.Analyzer.ListAsync("test");

            foreach (var analyzer in analyzers)
            {
                var def = await Arango.Analyzer.GetDefinitionAsync("test", analyzer.Name);
            }

            var views = await Arango.View.ListAsync("test");

            foreach (var view in views)
            {
                var props = await Arango.View.GetPropertiesAsync("test", view.Name);
            }

            await Arango.View.DropAllAsync("test");

            await Arango.Analyzer.DeleteAsync("test", "text_de_nostem", true);
        }
    }
}