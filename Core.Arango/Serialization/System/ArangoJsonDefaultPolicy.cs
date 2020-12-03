using System.Text.Json;

namespace Core.Arango.Serialization.System
{
    public class ArangoJsonDefaultPolicy : JsonNamingPolicy
    {
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