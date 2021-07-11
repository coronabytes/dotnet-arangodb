using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Core.Arango.Protocol
{
    /// <summary>
        ///   Arango Database
        /// </summary>
        public class ArangoDatabase
    {
        /// <summary>
        ///   Has to contain a valid database name.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Database Options
        /// </summary>
        [JsonPropertyName("options")]
        [JsonProperty(PropertyName = "options")]
        public ArangoDatabaseOptions Options { get; set; }

        /// <summary>
        /// Has to be an array of user objects to initially create for the new database. User information will not be changed for users that already exist. If users is not specified or does not contain any users, a default user root will be created with an empty string password. This ensures that the new database will be accessible after it is created.
        /// </summary>
        [JsonPropertyName("users")]
        [JsonProperty(PropertyName = "users")]
        public ICollection<ArangoUser> Users { get; set; }
    }
}