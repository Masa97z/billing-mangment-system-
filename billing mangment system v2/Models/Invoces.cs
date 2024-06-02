using MongoDB.Bson;

namespace billing_mangment_system_v2.Models
{
    public class Invoces
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public int Deps { get; set; }
        public int CurrentAmount { get; set; }
        public int TotalAmount { get; set; }
        public string CostumerId { get; set; }
    }
}
