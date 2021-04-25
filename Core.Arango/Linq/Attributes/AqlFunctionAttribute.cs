using System;

namespace Core.Arango.Linq.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AqlFunctionAttribute : Attribute
    {
        public AqlFunctionAttribute(string name)
        {
            Name = name;
        }

        public AqlFunctionAttribute(string name, bool isProperty)
        {
            Name = name;
            IsProperty = isProperty;
        }

        public string Name { get; }

        public bool IsProperty { get; }
    }
}