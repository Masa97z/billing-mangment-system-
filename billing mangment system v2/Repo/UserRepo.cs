using billing_mangment_system.Models;
using billing_mangment_system_v2.ICollectionService;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;
using billing_mangment_system_v2.Dtos;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace billing_mangment_system_v2.Repo
{
    public class UserRepo : IUserService
    {
        private readonly IMongoCollection<User> _user;
        private readonly IMongoCollection<UpdateUserDto> _user1;

        public UserRepo(IMongoDatabase database)

        {
            _user = database.GetCollection<User>("users");
        }

        public async Task<IEnumerable<User>> GetUserAsync()
        {
            var users = await _user.Find(_ => true).ToListAsync();
            return users;
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var user = await _user.Find(u => u.CostumerId == id).FirstOrDefaultAsync();
            return user;
        }
        public async Task<User> GetUserByCustomerIdAsync(string id)
        {
            var user = await _user.Find(u => u.CostumerId == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _user.InsertOneAsync(user);
            return user;
        }


        public async Task DeleteUserAsync(string id)
        {
            await _user.DeleteOneAsync(u => u.CostumerId == id);
        }

        public async Task UpdateUserById(string id, User user)
        {
            await _user.FindOneAndReplaceAsync(u => u.CostumerId == id, user);
        }

        public async Task UpdateUserByName(string name, User user)
        {
            await _user.ReplaceOneAsync(u => u.Name == name, user);
        }

        public async Task UpdateUserInvoceById(string id, User user)
        {
            await _user.ReplaceOneAsync(u => u.CostumerId == id, user);
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
