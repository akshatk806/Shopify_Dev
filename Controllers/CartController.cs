using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;
using Stripe;
using Stripe.Checkout;

namespace Product_Management.Controllers
{
    //[Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<UserModel> userManager;
        private readonly SignInManager<UserModel> signInManager;

        public CartController(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var userId = signInManager.UserManager.GetUserId(User);
            var cartItems = context.CartTable.Where(x => x.UserId == userId).ToList();
            var productList = context.Products.ToList();

            var finalList = cartItems.Join(
                                productList,
                                cart => cart.ProductRefId,
                                product => product.ProductId,
                                (cart, product) => new CartProductDTO()
                                {
                                    ProductId = product.ProductId,
                                    ProductName = product.ProductName,
                                    ProductImageURL = product.ProductImageURL,
                                    ProductPrice = product.ProductPrice,
                                    CartId = cart.CartId,
                                    Quantity = cart.Quantity,
                                    UserId = cart.UserId
                                }
                ).ToList();
                   


            return View(finalList);
        }

        [HttpGet]
        public async Task<IActionResult> Add([FromRoute] Guid id)
        {
            if (signInManager.IsSignedIn(User))
            {
                if (id != Guid.Empty)
                {
                    var user = signInManager.UserManager.GetUserId(User);
                    var userId = signInManager.UserManager.FindByIdAsync(user).Result.UserName;
                    var existingCart = context.CartTable.Where(x => x.UserId == user).FirstOrDefault(x => x.ProductRefId == id);

                    if (existingCart != null)
                    {
                        existingCart.Quantity++;
                    }
                    else
                    {
                        var newcart = new CartModel()
                        {
                            CartId = Guid.NewGuid(),
                            Quantity = 1,
                            ProductRefId = id,
                            UserId = user,
                        };
                        await context.CartTable.AddAsync(newcart);
                    }
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index", "Cart");
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Authentication");
        }

        [HttpGet]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id != Guid.Empty)
            {
                var CartItems = context.CartTable.FirstOrDefault(x => x.CartId == id);
                if (CartItems != null)
                {
                    context.CartTable.Remove(CartItems);
                    await context.SaveChangesAsync();
                }
            }
            TempData["cartDelete"] = "Product Removed from the Cart";
            return RedirectToAction("Index", "Cart");
        }


        public async Task<IActionResult> Reduce(Guid id)
        {
            if (id != Guid.Empty)
            {
                var item = context.CartTable.FirstOrDefault(x => x.CartId == id);
                if (item != null)
                {
                    if (item.Quantity > 1)
                    {
                        item.Quantity--;
                        await context.SaveChangesAsync();

                    }
                    else if (item.Quantity == 1)
                    {
                        return RedirectToAction("Delete", new { id = item.CartId });
                    }
                }
            }
            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> CheckOut()
        {
            var userId = signInManager.UserManager.GetUserId(User);
            var cartItems = await context.CartTable.Where(x => x.UserId == userId).ToListAsync();
            var productList = await context.Products.ToListAsync();

            var finalProductList = cartItems.Join(
                                productList,
                                cart => cart.ProductRefId,
                                product => product.ProductId,
                                (cart, product) => new CartProductDTO()
                                {
                                    ProductId = product.ProductId,
                                    ProductName = product.ProductName,
                                    ProductImageURL = product.ProductImageURL,
                                    ProductPrice = product.ProductPrice,
                                    CartId = cart.CartId,
                                    Quantity = cart.Quantity,
                                    UserId = cart.UserId
                                }
                ).ToList();

            var user = await userManager.GetUserAsync(User);
            string userEmail = user?.Email; 

            var domain = "https://localhost:7051/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Cart/OrderConfirmation",
                CancelUrl = domain + $"Cart/Index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = userEmail,
            };

            foreach (var product in finalProductList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = product.ProductPrice * 100,
                        Currency = "INR",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.ProductName.ToString()
                        },
                    },
                    Quantity = product.Quantity
                };
                
                options.LineItems.Add(sessionListItem);
            }

            var shippingFee = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 9900,
                    Currency = "INR",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Shipping Fee"
                    },
                },
                Quantity = 1
            };

            options.LineItems.Add(shippingFee);

            var service = new SessionService();
            Session session = service.Create(options);
            TempData["session"] = session.Id;

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            if (TempData["session"] == null)
            {
                return new StatusCodeResult(404);
            }

            Session session = service.Get(TempData["session"].ToString());

            if (session.PaymentStatus == "paid")
            {
                var transactionId = session.PaymentIntentId.ToString(); 
                return View("Success");
            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
