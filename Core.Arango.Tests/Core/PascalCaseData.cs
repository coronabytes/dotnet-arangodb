using Xunit;

namespace Core.Arango.Tests.Core
{
    public class PascalCaseData : TheoryData<string>
    {
        public PascalCaseData()
        {
            Add("newton-default");
            Add("system-default");
        }
    }
}