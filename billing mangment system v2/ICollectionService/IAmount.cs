using billing_mangment_system.Models;
using billing_mangment_system_v2.Models;

namespace billing_mangment_system_v2.ICollectionService
{
    public interface IAmount
    {
        Task<IEnumerable<Invoces>> GetInvocesAsync();
        Task<IEnumerable<Invoces>> GetInvocesByIdAsync(string id);
        Task<Invoces> CreateInvocesAsync(Invoces inv);
        Task UpdateInvocesAsync(string id, Invoces inv);
        Task DeleteInvocesAsync(string id);
    }
}
