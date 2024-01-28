using Microsoft.AspNetCore.Identity;

namespace Product_Management.Models.DomainModels
{
    public class UserModel : IdentityUser
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
