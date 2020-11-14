using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Arango.Serialization.Newtonsoft
{
    public class ArangoNewtonsoftCamelCaseContractResolver : DefaultContractResolver
    {
        public ArangoNewtonsoftCamelCaseContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy();
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
                "key" => "_key",
                "id" => "_id",
                "from" => "_from",
                "to" => "_to",
                _ => property.PropertyName
            };

            return property;
        }
    }
}