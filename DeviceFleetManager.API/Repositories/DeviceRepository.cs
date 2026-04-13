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
            
            // Normalizare query
            var normalized = new string(query.ToLower()
                .Select(c => char.IsPunctuation(c) ? ' ' : c).ToArray());
            
            var tokens = normalized
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(t => t != "gb" && t != "ram" && t != "mb")
                .ToArray();

            var scored = all
                .Select(device => new
                {
                    Device = device,
                    Score = tokens.Sum(token =>
                    {
                        int score = 0;
                        
                        // Name - max prior
                        var nameLower = device.Name?.ToLower() ?? "";
                        if (nameLower == token) score += 5;
                        else if (nameLower.Split(' ').Any(w => w == token)) score += 4;
                        else if (nameLower.Contains(token)) score += 3;
                        
                        // Manufacturer (3 pts exact, 2 pts contains)
                        var manuLower = device.Manufacturer?.ToLower() ?? "";
                        if (manuLower == token) score += 3;
                        else if (manuLower.Contains(token)) score += 2;
                        
                        // Processor (2 pts exact word, 1 pt contains)
                        var procLower = device.Processor?.ToLower() ?? "";
                        if (procLower.Split(' ').Any(w => w == token)) score += 2;
                        else if (procLower.Contains(token)) score += 1;
                        
                        // RAM - exact match pe numar (ex: "6" sau "6gb")
                        var ramStr = device.RamAmount.ToString();
                        var tokenDigits = new string(token.Where(char.IsDigit).ToArray());
                        if (!string.IsNullOrEmpty(tokenDigits) && ramStr == tokenDigits) score += 2;
                        
                        return score;
                    })
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .Select(x => x.Device)
                .ToList();

            return scored;
        }
                
    }
}