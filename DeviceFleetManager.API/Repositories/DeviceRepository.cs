using DeviceFleetManager.API.Models;
using MongoDB.Driver;

namespace DeviceFleetManager.API.Repositories
{
    public class DeviceRepository
    {
        private readonly IMongoCollection<Device> _devices;
        public DeviceRepository(IMongoDatabase database)
        {
            _devices = database.GetCollection<Device>("devices");
        }

        //all devices:
        public async Task<List<Device>> GetAllAsync() =>
            await _devices.Find(_ => true).ToListAsync();
        //device dupa id
        public async Task<Device?> GetByIdAsync(string id) =>
            await _devices.Find(d => d.Id == id).FirstOrDefaultAsync();
        
        //insert device  
        public async Task CreateAsync(Device device) =>
            await _devices.InsertOneAsync(device);

        //update
        public async Task UpdateAsync(string id, Device device) =>
            await _devices.ReplaceOneAsync(d => d.Id == id, device);

        //Delete
        public async Task DeleteAsync(string id) =>
            await _devices.DeleteOneAsync(d => d.Id == id);
                
    }
}