using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;

namespace Core.Arango.Tests
{
    public class FunctionTest : TestBase
    {
        private readonly IEqualityComparer<ArangoFunctionDefinition> _functionsComparer = new FunctionsComparer();

        public FunctionTest()
        {
            InitializeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Run(string serializer)
        {
            await SetupAsync(serializer);
            var function = Arango.Function;

            var isNewlyCreated = await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            Assert.True(isNewlyCreated);

            isNewlyCreated = await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            Assert.False(isNewlyCreated);

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn2",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn3",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "OtherTestfunctions::TestFn4",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            var result = await function.ListAsync("test");

            Assert.NotNull(result);

            AssertFunctionsList(new[]
            {
                "Testfunctions::TestFn1",
                "Testfunctions::TestFn2",
                "Testfunctions::TestFn3",
                "OtherTestfunctions::TestFn4"
            }, result);

            result = await function.ListAsync("test", "Testfunctions::");

            AssertFunctionsList(new[]
            {
                "Testfunctions::TestFn1",
                "Testfunctions::TestFn2",
                "Testfunctions::TestFn3"
            }, result);

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn2",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn3",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "OtherTestfunctions::TestFn4",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true
            });

            var exception1 = await Assert.ThrowsAsync<ArangoException>(async () =>
                await function.RemoveAsync("test", "BadFunctionName"));

            Assert.NotNull(exception1.ErrorNumber);
            Assert.NotNull(exception1.Code);
            Assert.Equal(ArangoErrorCode.ErrorQueryFunctionNotFound, exception1.ErrorNumber);
            Assert.Equal(HttpStatusCode.NotFound, exception1.Code);

            var exception2 = await Assert.ThrowsAsync<ArangoException>(async () =>
                await function.RemoveAsync("test", "Testfunctions::"));

            Assert.NotNull(exception2.ErrorNumber);
            Assert.NotNull(exception2.Code);
            Assert.Equal(ArangoErrorCode.ErrorQueryFunctionNotFound, exception2.ErrorNumber);
            Assert.Equal(HttpStatusCode.NotFound, exception2.Code);

            var deletedCount = await function.RemoveAsync("test", "BadNamespace", true);

            Assert.Equal(0, deletedCount);

            deletedCount = await function.RemoveAsync("test", "Testfunctions::TestFn3");

            Assert.Equal(1, deletedCount);

            var resultList = await function.ListAsync("test");

            Assert.NotNull(resultList);

            AssertFunctionsList(new[]
            {
                "Testfunctions::TestFn1",
                "Testfunctions::TestFn2",
                "OtherTestfunctions::TestFn4"
            }, resultList);

            deletedCount = await function.RemoveAsync("test", "Testfunctions::", true);

            Assert.Equal(2, deletedCount);

            resultList = await function.ListAsync("test");

            Assert.NotNull(resultList);

            AssertFunctionsList(new[]
            {
                "OtherTestfunctions::TestFn4"
            }, resultList);

            deletedCount = await function.RemoveAsync("test", "OtherTestfunctions::TestFn4");

            Assert.Equal(1, deletedCount);

            resultList = await function.ListAsync("test");

            Assert.NotNull(resultList);

            AssertFunctionsList(new string[0], resultList);
        }

        private void AssertFunctionsList(IList<string> expectedNames,
            IReadOnlyCollection<ArangoFunctionDefinition> list)
        {
            Assert.NotNull(expectedNames);
            Assert.NotNull(list);

            Assert.Equal(expectedNames.Count, list.Count);

            foreach (var example in expectedNames.Select(name => new ArangoFunctionDefinition {Name = name}))
                Assert.Contains(example, list, _functionsComparer);
        }

        private class FunctionsComparer : IEqualityComparer<ArangoFunctionDefinition>
        {
            public bool Equals(ArangoFunctionDefinition x, ArangoFunctionDefinition y)
            {
                return x?.Name?.Equals(y?.Name) ?? y?.Name == null;
            }

            public int GetHashCode(ArangoFunctionDefinition obj)
            {
                return obj?.Name?.GetHashCode() ?? 0;
            }
        }
    }
}