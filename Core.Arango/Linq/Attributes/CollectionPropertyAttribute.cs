using System;
using Core.Arango.Linq.Interface;

namespace Core.Arango.Linq.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CollectionPropertyAttribute : Attribute, ICollectionPropertySetting
    {
        public string CollectionName { get; set; }

        public NamingConvention Naming { get; set; }
    }
}