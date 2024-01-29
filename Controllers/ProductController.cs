using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var ProductList = await context.Products.ToListAsync();
            var OrderedProduct = ProductList.OrderByDescending(x => x.ProductCreatedAt).ToList();
            return View(OrderedProduct);
        }

        // Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductRequestDTO request)
        {
            var newProduct = new Product()
            {
                ProductId = Guid.NewGuid(),
                ProductName = request.ProductName,
                ProductDesc = request.ProductDesc,
                ProductPrice = request.ProductPrice,
                ProductCreatedAt = DateTime.Now
            };

            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();
            TempData["productsuccess"] = "Product Added Successfully";

            return RedirectToAction("Index", "Product");
        }

        //[HttpGet]
        //public IActionResult Update(Guid id)
        //{
        //    var product = context.Products.FirstOrDefault(x => x.ProductId == id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(product);
        //}


        // Update
        [HttpGet("/id")]
        public IActionResult Update(Guid id)
        {
            var product = context.Products.FirstOrDefault(x => x.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost("/id")]
        public async Task<IActionResult> Update(Product request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var existingProduct = await context.Products.FindAsync(request.ProductId);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.ProductId = request.ProductId;
            existingProduct.ProductName = request.ProductName;
            existingProduct.ProductDesc = request.ProductDesc;
            existingProduct.ProductPrice = request.ProductPrice;
            existingProduct.ProductCreatedAt = request.ProductCreatedAt;

            await context.SaveChangesAsync();
            TempData["productsuccess"] = "Product Updated Successfully";

            return RedirectToAction("Index", "Product");
        }
    }
}
