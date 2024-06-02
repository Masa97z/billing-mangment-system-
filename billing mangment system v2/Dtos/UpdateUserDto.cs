using billing_mangment_system.Models;

namespace billing_mangment_system_v2.Dtos
{
    public class UpdateUserDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //UniqueConstraint
        public string CostumerId { get; set; }  // give by admin 
        public string Phone { get; set; }
        public Address Address { get; set; }
    }
}
