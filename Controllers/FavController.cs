using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    public class FavController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<UserModel> userManager;
        private readonly SignInManager<UserModel> signInManager;

        public FavController(ApplicationDbContext context, UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var userId = signInManager.UserManager.GetUserId(User);
            var favItmes = context.FavTable.Where(x => x.UserId == userId).ToList();
            var productList = context.Products.ToList();

            var finalList = favItmes.Join(
                                productList,
                                fav => fav.ProductRefId,
                                product => product.ProductId,
                                (fav, product) => new FavProductDTO()
                                {
                                    ProductId = product.ProductId,
                                    ProductName = product.ProductName,
                                    ProductImageURL = product.ProductImageURL,
                                    ProductPrice = product.ProductPrice,
                                    FavId = fav.FavId,
                                    UserId = fav.UserId
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
                    var existingItem = context.FavTable.Where(x => x.UserId == user).FirstOrDefault(x => x.ProductRefId == id);

                    if (existingItem != null)
                    {
                        TempData["favProduct"] = "This Product is Already added in Favorites";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        var newFav = new FavModel()
                        {
                            FavId = Guid.NewGuid(),
                            ProductRefId = id,
                            UserId = user,
                        };
                        await context.FavTable.AddAsync(newFav);
                    }
                    await context.SaveChangesAsync();
                    return RedirectToAction("Index", "Fav");
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
                var FavItems = context.FavTable.FirstOrDefault(x => x.FavId == id);
                if (FavItems != null)
                {
                    context.FavTable.Remove(FavItems);
                    await context.SaveChangesAsync();
                }
            }
            TempData["cartDelete"] = "Product Removed from the Favorites";

            if (Convert.ToBoolean(TempData["deleteFromHomePage"]))
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Fav");
        }
    }
}
