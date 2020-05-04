using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class AnalyzerViewTest : TestBase
    {
        [Fact]
        public async Task AnalyzersViews()
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


            await Arango.CreateCollectionAsync("test", "collection", ArangoCollectionType.Document);

            await Arango.CreateViewAsync("test", new ArangoView
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

            await Arango.DropViewsAsync("test");

            await Arango.DeleteAnalyzerAsync("test", "text_de_nostem", true);
        }
    }
}