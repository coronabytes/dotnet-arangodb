using System;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Abstractions;

namespace Core.Arango.Tests
{
    public class DatabaseTest : TestBase
    {
        private readonly ITestOutputHelper _output;

        public DatabaseTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Create(string serializer)
        {
            await SetupAsync(serializer, null);

            await Arango.Database.CreateAsync("test");
            await Arango.Database.CreateAsync(new ArangoDatabase
            {
                Name = "test2"
            });
            await Arango.Database.ListAsync();

            Assert.True(await Arango.Database.ExistAsync("test"));
            Assert.True(await Arango.Database.ExistAsync("test2"));
            Assert.False(await Arango.Database.ExistAsync("test3"));

            var info = await Arango.Database.GetAsync("test");
            Assert.EndsWith("-test",info.Name);
        }
    }
}