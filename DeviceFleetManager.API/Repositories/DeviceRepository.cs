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
        
        public async Task<List<Device>> SearchAsync(string query){
            var all = await _devices.Find(_ => true).ToListAsync(); 
            
            var tokens = query.Trim().ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var scored = all
                .Select(device => new
                {
                    Device = device,
                    Score = tokens.Sum(token =>
                        (device.Name?.ToLower().Contains(token) == true ? 4 : 0) +
                        (device.Manufacturer?.ToLower().Contains(token) == true ? 3 : 0) +
                        (device.Processor?.ToLower().Contains(token) == true ? 2 : 0) +
                        (device.RamAmount.ToString().Contains(token) == true ? 1 : 0)
                    )
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Device)
                .ToList();

            return scored;
        }
                
    }
}