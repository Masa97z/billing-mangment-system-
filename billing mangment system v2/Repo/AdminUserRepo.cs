using billing_mangment_system.Models;
using billing_mangment_system_v2.ICollectionService;
using billing_mangment_system_v2.Models;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace billing_mangment_system_v2.Repo
{
    public class AdminUserRepo : IAdminUser
    {
        private readonly IMongoCollection<AdminUsers> _user;

        public AdminUserRepo(IMongoDatabase database)
        {
            _user = database.GetCollection<AdminUsers>("Adminusers");
        }

        public async Task<IEnumerable<AdminUsers>> GetUserAsync()
        {
            var users = await _user.Find(_ => true).ToListAsync();
            return users;
        }

        public async Task<AdminUsers> GetadminUserByIdAsync(string id)
        {
            var user = await _user.Find(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }
       
       
        public async Task<AdminUsers> CreateAdminUserAsync(AdminUsers user)
        {
            await _user.InsertOneAsync(user);
            return user;
        }


        public async Task DeleteUserAsync(string id)
        {
            await _user.DeleteOneAsync(u => u.Id == id);
        }

        public async Task UpdateUserById(string id, AdminUsers user)
        {
            await _user.FindOneAndReplaceAsync(u => u.Id == id, user);
        }

       
    }
}
