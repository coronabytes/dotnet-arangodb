using System;

namespace Core.Arango.Linq.Query
{
    internal class ExtentionIdentifierAttribute : Attribute
    {
        public ExtentionIdentifierAttribute(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; set; }
    }
}