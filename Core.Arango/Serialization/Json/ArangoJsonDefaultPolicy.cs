using System.Text.Json;

namespace Core.Arango.Serialization.Json
{
    /// <summary>
    ///   System.Json.Text PascalCase Naming Policy for Arango
    /// </summary>
    public class ArangoJsonDefaultPolicy : JsonNamingPolicy
    {
        /// <inheritdoc/>
        public override string ConvertName(string name)
        {
            return name switch
            {
                "Id" => "_id",
                "Key" => "_key",
                "Revision" => "_rev",
                "From" => "_from",
                "To" => "_to",
                _ => name
            };
        }
    }
}