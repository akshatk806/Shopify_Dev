using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    [Route("Order")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly SignInManager<UserModel> signInManager;
        public OrderController(ApplicationDbContext context, SignInManager<UserModel> signInManager)
        {
            this.context = context;
            this.signInManager = signInManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index([FromRoute] string Id)
        {
            if (Id != signInManager.UserManager.GetUserId(User))
            {
                return new StatusCodeResult(404);
            }
            var orders = await context.Orders
                        .Join(context.Products,
                              order => order.ProductRefId,
                              product => product.ProductId,
                              (order, product) => new OrderViewModelDTO
                              {
                                  OrderId = order.OrderId,
                                  ProductPrice = order.ProductPrice,
                                  TransactionId = order.TransactionId!,
                                  ProductQuantity = order.ProductQuantity,
                                  UserId = order.UserId,
                                  Date = order.Date,
                                  ProductName = product.ProductName,
                                  ProductCategory = product.Category.CategoryName
                              })
                        .Where(x => x.UserId == Id)
                        .OrderByDescending(x => x.Date)
                        .ToListAsync();

            return View(orders);
        }
    }
}
