using DeviceFleetManager.API.Models;
using DeviceFleetManager.API.Repositories;

namespace DeviceFleetManager.API.Services
{
    public class DeviceService
    {
        private readonly DeviceRepository _repository;
        public DeviceService(DeviceRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Device>> GetAllAsync() =>
            await _repository.GetAllAsync();
        
         public async Task<Device?> GetByIdAsync(string id) =>
            await _repository.GetByIdAsync(id);
        
        public async Task CreateAsync(Device device) =>
            await _repository.CreateAsync(device);  
         public async Task UpdateAsync(string id, Device device) =>
            await _repository.UpdateAsync(id, device);

        public async Task DeleteAsync(string id) =>
            await _repository.DeleteAsync(id);
        public async Task<List<Device>> SearchAsync(string query){
            return await _repository.SearchAsync(query);
        }
        
    }
}