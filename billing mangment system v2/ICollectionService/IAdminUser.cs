using billing_mangment_system.Models;
using billing_mangment_system_v2.Models;

namespace billing_mangment_system_v2.ICollectionService
{
    public interface IAdminUser
    {
        Task<IEnumerable<AdminUsers>> GetUserAsync();
        Task<AdminUsers> GetadminUserByIdAsync(string id);
        Task<AdminUsers> CreateAdminUserAsync(AdminUsers adminUser);
        Task UpdateUserById(string id, AdminUsers adminUser);

        Task DeleteUserAsync(string id);
    }
}
