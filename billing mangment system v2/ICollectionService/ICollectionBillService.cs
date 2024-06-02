using billing_mangment_system.Models;
using MongoDB.Driver;

public interface ICollectionBillService
{
    Task<IEnumerable<Bills>> GetBillsAsync();
    Task<IEnumerable<Bills>> GetBillByIdAsync(string id);
    Task<Bills> CreateBillAsync(Bills bill);
    Task UpdateBillAsync(string id, Bills bill);
    Task DeleteBillAsync(string id);
}

