using System.Text.Json;

namespace Core.Arango.Serialization.System
{
    public class ArangoJsonDefaultPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name switch
            {
                "Key" => "_key",
                "Id" => "_id",
                "From" => "_from",
                "To" => "_to",
                _ => name
            };
        }
    }
}