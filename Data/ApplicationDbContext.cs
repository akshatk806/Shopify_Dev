using Microsoft.EntityFrameworkCore;
using Product_Management.Models.DomainModels;

namespace Product_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CartModel> CartTable { get; set; }

        public DbSet<FavModel> FavTable { get; set; }
    }
}
