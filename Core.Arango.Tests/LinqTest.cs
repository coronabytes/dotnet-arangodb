using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Linq;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class LinqTest : TestBase
    {
        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Test1(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", nameof(Project), ArangoCollectionType.Document);
            await Arango.Collection.CreateAsync("test", nameof(Activity), ArangoCollectionType.Document);

            var (aql, bindVars) = Arango.Query<Project>("test")
                .Where(x => x.Name == "A")
                .Take(1)
                .ToAql();

            aql.ToString();
        }
    }
}