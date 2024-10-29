using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class QueryFormattingTest : TestBase
    {
        public QueryFormattingTest()
        {
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task GuidFormat(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var userKey = Guid.Parse("{867E46A9-3623-4D7B-B33A-9E24F0E7D660}");
            var clientKey = Guid.Parse("{8038F24B-AF91-4E7D-BAFE-1D96DF7FD06A}");

            var user = await Arango.Query.SingleOrDefaultAsync<Entity>("test", "test",
                $"x._key == {userKey} && x.ClientKey == {clientKey}");
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task DateTimeFormat(string serializer)
        {
            await SetupAsync(serializer);
            await Arango.Collection.CreateAsync("test", "test", ArangoCollectionType.Document);

            var date = new DateTime(2020, 12, 4, 9, 58, 25, DateTimeKind.Utc);

            var resultDefault = (await Arango.Query.ExecuteAsync<string>("test", $"LET v = {date} RETURN v"))
                .SingleOrDefault();

            Assert.Equal("2020-12-04T09:58:25Z", resultDefault);
        }
    }
}