using billing_mangment_system.Models;
using billing_mangment_system_v2.Dtos;
using MongoDB.Driver;

namespace billing_mangment_system_v2.ICollectionService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUserAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserById(string id, User user);
        Task<User> GetUserByCustomerIdAsync(string id);

        Task DeleteUserAsync(string id);
    }

   
}
