using MongoDB.Bson;

namespace billing_mangment_system.Models
{

    
    public class Bills
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public DateTime PreTime { get; set; } 
        public int PreUnit { get; set; } = 0;
        public DateTime PostTime { get; set; }
        public int PostUnit { get; set; } = 0;
        public string CostumerId { get; set; }  // Reference to User
    }
    
}
