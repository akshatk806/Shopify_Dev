using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Product_Management.Models.DomainModels
{
    public class UserModel : IdentityUser
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime UserAddedAt { get; set; } = DateTime.Now;
    }
}
