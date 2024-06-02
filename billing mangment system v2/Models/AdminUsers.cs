using MongoDB.Bson;

namespace billing_mangment_system_v2.Models
{
    public class AdminUsers
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //UniqueConstraint
        public string regonT { get; set; } // 1 to home 2 to gov 3 to comm 4 to indus
        public long Phone { get; set; }
    }
}
