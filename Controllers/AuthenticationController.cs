using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Constants;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;
using Product_Management.Services;
using System.Security.Claims;
using System.Web;

namespace Product_Management.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly SignInManager<UserModel> signInManager;
        private readonly UserManager<UserModel> userManager;
        private readonly AuthDbContext context;
        private readonly EmailSender emailContext;

        public AuthenticationController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, AuthDbContext context, EmailSender emailSender)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
            this.emailContext = emailSender;
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
                request.Name = HttpUtility.HtmlEncode(request.Name);
                request.Email = HttpUtility.HtmlEncode(request.Email);
                request.Address = HttpUtility.HtmlEncode(request.Address);
                request.Phone = HttpUtility.HtmlEncode(request.Phone);
                request.Password = HttpUtility.HtmlEncode(request.Password);

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

                    string subject = "Welcome to Shopify! Let's Get Started 🚀";

                    string body = $"<p>Dear {request.Name},</p>\r\n<p><br></p>\r\n<p>Welcome to " +
                        "Shopify! We&apos;re thrilled to have you join our community of shoppers. Get ready" +
                        " to discover amazing products and enjoy a seamless shopping experience right " +
                        "from the comfort of your home.</p>\r\n<p><br></p>\r\n<p>To help you get started, " +
                        "here are a few quick steps to make the most of your shopping experience:</p>" +
                        "\r\n<p><br></p>\r\n<p>1. Explore Our Products: Dive into our wide range of " +
                        "products carefully curated to meet your needs and preferences." +
                        " Whether you&apos;re looking for trendy fashion pieces, unique accessories, " +
                        "or must-have gadgets, we&apos;ve got you covered.</p>\r\n<p><br></p>\r\n<p>" +
                        "2. Create Your Wishlist: Found something you love? Save it to your wishlist for" +
                        " easy access later. Simply click the heart icon next to the product to add it " +
                        "to your list.</p>\r\n<p><br></p>\r\n<p>3. Stay Updated: Be the first to know " +
                        "about exclusive offers, new arrivals, and exciting promotions by subscribing " +
                        "to our newsletter. You won&apos;t want to miss out on our special deals!</p>\r\n" +
                        "<p><br></p>\r\n<p>4. Easy Checkout Process: Enjoy a hassle-free checkout " +
                        "experience with Shopify&apos;s secure payment system. Your privacy and security" +
                        " are our top priorities.</p>\r\n<p><br></p>\r\n<p>And here&apos;s your login " +
                        $"information:</p>\r\n<p>Your Password: &quot; {request.Password} &quot;</p>" +
                        "\r\n<p><br></p>\r\n<p>And that&apos;s it! You&apos;re all set to start shopping. " +
                        "We hope you have a fantastic experience with us. Thank you for choosing Shopify." +
                        "</p>\r\n<p><br></p>\r\n<p>Happy shopping!</p>\r\n<p><br></p>\r\n<p>Best regards," +
                        "</p>\r\n<p>Shopify Admin</p>";

                    // await emailContext.SendEmail(request.Email, subject, body);

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

        // google login
        [HttpGet]
        public IActionResult GoogleLogin(string returnUrl = "/")
        {
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Authentication", new { returnUrl });
            var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // Create user if not exist
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                var user = await userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    user = new UserModel
                    {
                        UserName = email,
                        Email = email,
                        Name = info.Principal.FindFirstValue(ClaimTypes.Name)
                    };
                    var result = await userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        TempData["deactiveLogin"] = "User creation failed!";
                        return RedirectToAction(nameof(Login));
                    }
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
                }

                await userManager.AddLoginAsync(user, info);
                await signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
        }

    }
}
