using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Migrations.ApplicationDb;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace CustomIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<UserModel> signInManager;
        private readonly UserManager<UserModel> userManager;
        private readonly AuthDbContext context;

        public AdminController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, AuthDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var allUsers = userManager.Users.Where(x => !x.Email.Contains("admin")).OrderByDescending(x => x.UserAddedAt).ToList();

            return View(allUsers);
        }

        [HttpGet("/id")]
        public IActionResult UpdateUser(string id)
        {
            var user = userManager.Users.FirstOrDefault(x => x.Id == id);

            return View(user);
        }

        [HttpPost("/id")] 
        public IActionResult UpdateUser(UserModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var existingUser = userManager.Users.FirstOrDefault(x => x.Id == request.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Email = request.Email;
            existingUser.Password = request.Password;
            existingUser.Name = request.Name;
            existingUser.Address = request.Address; 
            existingUser.Phone = request.Phone;

            context.SaveChanges();
            TempData["usersuccess"] = "User Updated Successfully";
            return RedirectToAction("GetAllUsers", "Admin");
        }

        [HttpGet]
        public IActionResult DeactiveUser(string id)
        {
            var user = userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = false;
            context.SaveChanges();
            return RedirectToAction("GetAllUsers", "Admin");
        } 
        [HttpGet]
        public IActionResult ActivateUser(string id)
        {
            var user = userManager.Users.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsActive = true;
            context.SaveChanges();
            return RedirectToAction("GetAllUsers", "Admin");
        }
    }
}
