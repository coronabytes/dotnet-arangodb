namespace Core.Arango.Tests
{
    public abstract class TestBase
    {
        protected readonly ArangoContext Arango = 
            new ArangoContext("Server=http://localhost:8529;Realm=unittest;User ID=root;Password=;");
    }
}