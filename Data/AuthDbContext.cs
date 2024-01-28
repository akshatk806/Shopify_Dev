using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Product_Management.Models.DomainModels;

namespace Product_Management.Data
{
    public class AuthDbContext : IdentityDbContext<UserModel>
    {
        public AuthDbContext(DbContextOptions options) : base (options)
        {
            
        }
    }
}
