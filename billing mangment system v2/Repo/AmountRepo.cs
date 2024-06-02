using billing_mangment_system.Models;
using billing_mangment_system_v2.ICollectionService;
using billing_mangment_system_v2.Models;
using MongoDB.Driver;

namespace billing_mangment_system_v2.Repo
{
    public class AmountRepo : IAmount
    {
        private readonly IMongoCollection<Invoces> _inv;

        public AmountRepo(IMongoDatabase database)
        {

            _inv = database.GetCollection<Invoces>("Invoces");
        }
        public async Task<Invoces> CreateInvocesAsync(Invoces inv)
        {
            await _inv.InsertOneAsync(inv);
            return inv;
        }

        public async   Task DeleteInvocesAsync(string id)
        {
            await _inv.DeleteOneAsync(i => i.CostumerId == id);
        }

        public async Task<IEnumerable<Invoces>> GetInvocesAsync()
        {
            return await _inv.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Invoces>> GetInvocesByIdAsync(string id)
        {
            return await _inv.Find(invoce => invoce.CostumerId == id).ToListAsync();
        }

        public async Task UpdateInvocesAsync(string id, Invoces inv)
        {
            await _inv.ReplaceOneAsync(i => i.CostumerId == id, inv);
        }
    }
}
