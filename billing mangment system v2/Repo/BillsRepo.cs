using billing_mangment_system.Models;
using MongoDB.Driver;

namespace billing_mangment_system_v2.Repo
{
    public class BillsRepo : ICollectionBillService
    {
        private readonly IMongoCollection<Bills> _bills;

        public BillsRepo(IMongoDatabase database)
        {

            _bills = database.GetCollection<Bills>("CollectionBills");
        }

        public async Task<IEnumerable<Bills>> GetBillsAsync()
        {
            return await _bills.Find(_ => true).ToListAsync();
        }

        public async Task<IEnumerable<Bills>> GetBillByIdAsync(string id)
        {
            return await _bills.Find(bill => bill.CostumerId == id).ToListAsync();
        }

        public async Task<Bills> CreateBillAsync(Bills bill)
        {

            await _bills.InsertOneAsync(bill);
            return bill;
        }

        public async Task UpdateBillAsync(string id, Bills bill)
        {
            await _bills.ReplaceOneAsync(b => b.CostumerId == id, bill);
        }

        public async Task DeleteBillAsync(string id)
        {
            await _bills.DeleteOneAsync(bill => bill.CostumerId == id);
        }

    }
}
