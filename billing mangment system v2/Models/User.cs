using billing_mangment_system_v2.Models;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace billing_mangment_system.Models
{
    public class User
    {
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //UniqueConstraint
        public string typeAccount { get; set; } // 1 to home 2 to gov 3 to comm 4 to indus
        public string CostumerId { get; set; }  // give by admin 
        public string Phone { get; set; }
        public Address Address { get; set; }

    }
}
