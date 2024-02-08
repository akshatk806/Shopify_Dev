using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Models.DomainModels;

namespace Product_Management.Controllers
{
    [Route("Profile")]
    public class ProfileController : Controller
    {
        private readonly AuthDbContext context;
        public ProfileController(AuthDbContext context)
        {
            this.context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Index([FromRoute] string Id)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == Id);
            if(user != null)
            {
                var profile = new UserModel()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    Password = user.Password,
                    UserAddedAt = user.UserAddedAt
                };
                return View(profile);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
