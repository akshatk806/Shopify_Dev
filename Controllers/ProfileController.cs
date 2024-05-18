using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Models.DomainModels;

namespace Product_Management.Controllers
{
    [Route("Profile")]
    public class ProfileController : Controller
    {
        private readonly AuthDbContext context;
        private readonly SignInManager<UserModel> signInManager;
        public ProfileController(AuthDbContext context, SignInManager<UserModel> signInManager)
        {
            this.context = context;
            this.signInManager = signInManager; 
        }

        [HttpGet("{id}")]
        public IActionResult Index([FromRoute] string Id)
        {
            if (Id != signInManager.UserManager.GetUserId(User))
            {
                return new StatusCodeResult(404);
            }

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
