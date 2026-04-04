using DeviceFleetManager.API.Models;
using DeviceFleetManager.API.Repositories;
using MongoDB.Driver;

namespace DeviceFleetManager.API.Services
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _users;
        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        //all users
        public async Task<List<User>> GetAllAsync() =>
            await _users.Find(_ => true).ToListAsync();
        //find dupa id
        public async Task<User?> GetByIdAsync(string id) =>
            await _users.Find(u => u.Id ==id).FirstOrDefaultAsync();
        
        //insert user
        public async Task CreateAsync(User user) =>
            _users.InsertOneAsync(user);
        
        //update
        public async Task UpdateAsync(string id, User user) =>
            _users.ReplaceOneAsync(u => u.Id ==id, user);
        
        //Delete
        public async Task DeleteAsync(string id) =>
            _users.DeleteOneAsync(u => u.Id == id);

    }
    
}