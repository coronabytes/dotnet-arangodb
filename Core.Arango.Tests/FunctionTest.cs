using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Priority;

namespace Core.Arango.Tests
{
    public class FunctionTest : TestBase
    {
        private readonly IEqualityComparer<ArangoFunctionDefinition> _functionsComparer = new FunctionsComparer();

        [Theory]
        [ClassData(typeof(PascalCaseData))]
        public async Task Run(IArangoContext arango)
        {
            await SetupAsync(arango);
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

            await Assert.ThrowsAsync<ArangoException>(async () =>
                await function.RemoveAsync("test", "BadFunctionName"));

            await Assert.ThrowsAsync<ArangoException>(async () =>
                await function.RemoveAsync("test", "Testfunctions::"));

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