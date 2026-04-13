using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeviceFleetManager.API.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]  //primary key
        [BsonRepresentation(BsonType.ObjectId)] //generare automata a id urilor.
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        
    }
}