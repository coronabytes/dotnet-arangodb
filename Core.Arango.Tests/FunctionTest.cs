using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Core.Arango.Protocol;
using Core.Arango.Tests.Core;
using Xunit;
using Xunit.Priority;

namespace Core.Arango.Tests
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly), DefaultPriority(-1000)]
    public class FunctionTest : TestBase
    {
        private IEqualityComparer<ArangoFunctionDefinition> functionsComparer = new FunctionsComparer();

        [Fact, Priority(0)]
        public void ModuleExists()
        {
            var function = Arango.Function;

            Assert.NotNull(function);
        }

        [Fact, Priority(1000)]
        public async Task CreateTest()
        {
            var function = Arango.Function;

            var result = await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            Assert.NotNull(result);
            Assert.False(result.Error);
            Assert.True(result.IsNewlyCreated);

            result = await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            Assert.NotNull(result);
            Assert.False(result.Error);
            Assert.False(result.IsNewlyCreated);
        }

        [Fact, Priority(2000)]
        public async Task ListTest()
        {
            var function = Arango.Function;

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn2",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn3",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "OtherTestfunctions::TestFn4",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            var result = await function.ListAsync("test");

            Assert.NotNull(result);

            Assert.False(result.Error);

            AssertFunctionsList(new[]{
                "Testfunctions::TestFn1",
                "Testfunctions::TestFn2",
                "Testfunctions::TestFn3",
                "OtherTestfunctions::TestFn4",
            }, result.Result);

            result = await function.ListAsync("test", "Testfunctions::");

            AssertFunctionsList(new[]{
                "Testfunctions::TestFn1",
                "Testfunctions::TestFn2",
                "Testfunctions::TestFn3",
            }, result.Result);
        }

        [Fact, Priority(3000)]
        public async Task RemoveTest()
        {
            var function = Arango.Function;

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn1",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn2",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "Testfunctions::TestFn3",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await function.CreateAsync("test", new ArangoFunctionDefinition
            {
                Name = "OtherTestfunctions::TestFn4",
                Code = "function (a) { return a * 2; }",
                IsDeterministic = true,
            });

            await Assert.ThrowsAsync<ArangoException>(async () => await function.RemoveAsync("test", "BadFunctionName"));

            await Assert.ThrowsAsync<ArangoException>(async () => await function.RemoveAsync("test", "Testfunctions::"));

            var result = await function.RemoveAsync("test", new FunctionRemoveRequest
            {
                Name = "BadNamespace",
                Group = true,
            });

            Assert.NotNull(result);
            Assert.Equal(0, result.DeletedCount);

            result = await function.RemoveAsync("test", "Testfunctions::TestFn3");

            Assert.NotNull(result);
            Assert.Equal(1, result.DeletedCount);

            var resultList = await function.ListAsync("test");

            Assert.NotNull(resultList);
            Assert.False(resultList.Error);

            AssertFunctionsList(new[]{
                "Testfunctions::TestFn1",
                "Testfunctions::TestFn2",
                "OtherTestfunctions::TestFn4",
            }, resultList.Result);

            result = await function.RemoveAsync("test", new FunctionRemoveRequest
            {
                Name = "Testfunctions::",
                Group = true,
            });

            Assert.NotNull(result);
            Assert.Equal(2, result.DeletedCount);

            resultList = await function.ListAsync("test");

            Assert.NotNull(resultList);
            Assert.False(resultList.Error);

            AssertFunctionsList(new[]{
                "OtherTestfunctions::TestFn4",
            }, resultList.Result);

            result = await function.RemoveAsync("test", "OtherTestfunctions::TestFn4");

            Assert.NotNull(result);
            Assert.Equal(1, result.DeletedCount);

            resultList = await function.ListAsync("test");

            Assert.NotNull(resultList);
            Assert.False(resultList.Error);

            AssertFunctionsList(new string[0], resultList.Result);
        }

        private void AssertFunctionsList(IList<string> expectedNames, IList<ArangoFunctionDefinition> list)
        {
            Assert.NotNull(expectedNames);
            Assert.NotNull(list);

            Assert.Equal(expectedNames.Count, list.Count);

            foreach (var example in expectedNames.Select(name => new ArangoFunctionDefinition { Name = name }))
                Assert.Contains(example, list, functionsComparer);
        }

        private class FunctionsComparer : IEqualityComparer<ArangoFunctionDefinition>
        {
            public bool Equals(ArangoFunctionDefinition x, ArangoFunctionDefinition y)
                => x?.Name?.Equals(y?.Name) ?? (y?.Name == null);

            public int GetHashCode(ArangoFunctionDefinition obj)
                => obj?.Name?.GetHashCode() ?? 0;
        }
    }
}