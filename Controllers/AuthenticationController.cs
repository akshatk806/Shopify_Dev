using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Constants;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<UserModel> signInManager;
        private readonly UserManager<UserModel> userManager;
        private readonly AuthDbContext context;

        public AuthenticationController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, AuthDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
        }

        // Login
        [HttpGet]
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            else if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            if(user == null)
            {
                TempData["deactiveLogin"] = "Invalid User!";
                return RedirectToAction("Login", "Authentication");
            }
            if (user.IsActive == false && user.Email.Contains("admin") == false)
            {
                TempData["deactiveLogin"] = "Your account has been deactivated by the admin!";
                return RedirectToAction("Login", "Authentication");
            }
            if (ModelState.IsValid)
            {
                //login
                var result = await signInManager.PasswordSignInAsync(request.Email!, request.Password!, request.RememberMe, false);
                if (result.Succeeded)
                {
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    if (User.IsInRole("User"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login attempt");
                    return View(request);
                }
            }
            return View(request);
        }





        // Register
        public IActionResult Register()
        {
            if(User.IsInRole("Admin"))
            {
                return View();
            }
            else if(User.IsInRole("User"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();   
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel()
                {
                    UserName = request.Email,
                    Name = request.Name,
                    Email = request.Email,
                    Address = request.Address,
                    Phone = request.Phone,
                    Password = request.Password,
                    IsActive = request.IsActive
                };
                var result = await userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        TempData["usersuccess"] = "User Added Successfully";
                        return RedirectToAction("GetAllUsers", "Admin");
                    }
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(request);
        }


        // Logout
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
