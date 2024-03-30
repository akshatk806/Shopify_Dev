using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Services;

namespace CustomIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<UserModel> signInManager;
        private readonly UserManager<UserModel> userManager;
        private readonly AuthDbContext context;
        private readonly ApplicationDbContext dbContext;
        private readonly EmailSender emailContext;

        public AdminController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, AuthDbContext context, ApplicationDbContext dbContext, EmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
            this.dbContext = dbContext;
            this.emailContext = emailSender;
        }
        public IActionResult Index()
        {
            ViewBag.totalUsers = context.Users.Where(x => !x.Email.Contains("Admin") && x.Phone != "NA").Count();
            ViewBag.totalProducts = dbContext.Products.Count();
            return View();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var allUsers = userManager.Users.Where(x => !x.Email.Contains("admin") && x.Phone != "NA").OrderByDescending(x => x.UserAddedAt).ToList();
            
            return View(allUsers);
        }

        [HttpGet("/id")]
        public IActionResult UpdateUser(string id)
        {
            var user = userManager.Users.FirstOrDefault(x => x.Id == id);
            if(user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost("/id")] 
        public async Task<IActionResult> UpdateUser(UserModel request)
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

            var oldPassword = existingUser.Password;
            var newPassword = request.Password;

            existingUser.Email = request.Email;
            existingUser.Password = request.Password;
            existingUser.Name = request.Name;
            existingUser.Address = request.Address; 
            existingUser.Phone = request.Phone;

            var resultPassword = await userManager.ChangePasswordAsync(existingUser, oldPassword, newPassword);
            if (resultPassword.Succeeded)
            {
                existingUser.Password = newPassword;

                await userManager.UpdateAsync(existingUser);
            }

            context.SaveChanges();
            if (oldPassword != newPassword)
            {
                await emailContext.SendEmail(request.Email, "Your Password is changed by the Shopify Admin", "\nNew Password: " + existingUser.Password);
            }
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
