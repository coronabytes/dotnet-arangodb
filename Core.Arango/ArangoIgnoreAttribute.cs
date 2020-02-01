using System;

namespace Core.Arango
{
    /// <summary>
    ///     Mark properties to be ignored from being written to ArangoDB
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArangoIgnoreAttribute : Attribute
    {
    }
}