using Xunit;

namespace Core.Arango.Tests.Core
{
    public class CamelCaseData : TheoryData<string>
    {
        public CamelCaseData()
        {
            Add("newton-camel");
            Add("system-camel");
        }
    }
}