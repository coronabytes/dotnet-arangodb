using System;

namespace Core.Arango.Linq.Attributes
{
    /// <summary>
    ///  LINQ Arango Function
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AqlFunctionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public AqlFunctionAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public AqlFunctionAttribute(string name, bool isProperty)
        {
            Name = name;
            IsProperty = isProperty;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsProperty { get; }
    }
}