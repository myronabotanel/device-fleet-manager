using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DeviceFleetManager.API.Models
{
    public class Device
    {
        [BsonId]  //primary key
        [BsonRepresentation(BsonType.ObjectId)] //generare automata a id urilor.
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // phone, tablet
        public string OperatingSystem { get; set; } = string.Empty;
        public string OsVersion { get; set; } = string.Empty;
        public string Processor { get; set; } = string.Empty;
        public int RamAmount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? UserId { get; set; } //pt cine foloseste dispozitivul

    }
}