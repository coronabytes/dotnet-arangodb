using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Arango
{
    /// <summary>
    ///     Json.NET Contract Resolver for translating ArangoDB keywords
    /// </summary>
    internal class ArangoContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<ArangoIgnoreAttribute>() != null)
            {
                property.ShouldDeserialize = i => true;
                property.ShouldSerialize = i => false;
            }

            if (property.PropertyName == "Key")
                property.PropertyName = "_key";
            else if (property.PropertyName == "Id")
                property.PropertyName = "_id";
            else if (property.PropertyName == "From")
                property.PropertyName = "_from";
            else if (property.PropertyName == "To")
                property.PropertyName = "_to";

            return property;
        }
    }
}