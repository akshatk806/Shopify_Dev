using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Constants;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<UserModel> signInManager;
        private readonly UserManager<UserModel> userManager;

        public AuthenticationController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
