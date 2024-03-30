using Microsoft.AspNetCore.Identity;
using Product_Management.Constants;
using Product_Management.Models.DomainModels;
using System.Data;

namespace Product_Management.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider services)
        {
            // seed Roles
            var userManagar = services.GetService<UserManager<UserModel>>();
            var roleManager = services.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // creating admin role
            var user = new UserModel()
            {
                UserName = "admin@shopify.in",
                Email = "admin@shopify.in",
                Name = "Akshat",
                Address = "Noida Office",
                Phone = "NA",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Password = "Admin@123"
            };

            var isUserExists = await userManagar.FindByEmailAsync(user.Email);
            if (isUserExists == null)
            {
                await userManagar.CreateAsync(user, "Admin@123");
                await userManagar.CreateAsync(user, Roles.Admin.ToString());
            }
        }
    }
}
