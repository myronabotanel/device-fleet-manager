using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeviceFleetManager.API.Models
{
    public class User
    {
        [BsonId] //primary key
        [BsonRepresentation(BsonType.ObjectId)] //generare automata de id
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}