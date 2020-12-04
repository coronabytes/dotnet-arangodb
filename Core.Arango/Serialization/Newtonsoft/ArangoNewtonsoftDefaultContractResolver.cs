using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Arango.Serialization.Newtonsoft
{
    /// <summary>
    ///     Json.NET Contract Resolver for translating ArangoDB keywords
    /// </summary>
    public class ArangoNewtonsoftDefaultContractResolver : DefaultContractResolver
    {
        public ArangoNewtonsoftDefaultContractResolver()
        {
            NamingStrategy = new DefaultNamingStrategy();
        }
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<ArangoIgnoreAttribute>() != null)
            {
                property.ShouldDeserialize = i => true;
                property.ShouldSerialize = i => false;
            }

            property.PropertyName = property.PropertyName switch
            {
                "Id" => "_id",
                "Key" => "_key",
                "Revision" => "_rev",
                "From" => "_from",
                "To" => "_to",
                _ => property.PropertyName
            };

            return property;
        }
    }
}