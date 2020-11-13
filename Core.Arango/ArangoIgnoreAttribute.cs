using System;

namespace Core.Arango
{
    /// <summary>
    ///     Json.NET Only - Mark properties to be ignored from being written to ArangoDB
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArangoIgnoreAttribute : Attribute
    {
    }
}