using DeviceFleetManager.API.Models;
using DeviceFleetManager.API.Repositories;

namespace DeviceFleetManager.API.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> GetAllAsync() =>
            await _repository.GetAllAsync();
        
         public async Task<User?> GetByIdAsync(string id) =>
            await _repository.GetByIdAsync(id);
        
        public async Task CreateAsync(User user) =>
            await _repository.CreateAsync(user);  
        public async Task UpdateAsync(string id, User user) =>
            await _repository.UpdateAsync(id, user);

        public async Task DeleteAsync(string id) =>
            await _repository.DeleteAsync(id);
        
    }
}