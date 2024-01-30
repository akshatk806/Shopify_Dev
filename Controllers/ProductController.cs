using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("Product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;
        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet("/AllProducts")]
        public async Task<IActionResult> Index()
        {
            /*
            var ProductList = await context.Products.ToListAsync();
            context.Products.Include(x => x.Category).Include(x => x.CategoryId);
            var OrderedProduct = ProductList.OrderByDescending(x => x.ProductCreatedAt).ToList();
            return View(OrderedProduct);
            */

            var productList = await context.Products.ToListAsync();
            List<Category> categoryList = await context.Categories.ToListAsync();
            var finalProduct = productList.Join(
                            categoryList,
                            product => product.CategoryId,
                            category => category.CategoryId,
                            (product, category) => new ProductCategoryDTO
                            {
                                ProductId = product.ProductId,
                                ProductName = product.ProductName,
                                ProductDesc = product.ProductDesc,
                                ProductPrice = product.ProductPrice,
                                ProductCreatedAt = product.ProductCreatedAt,
                                CategoryId = category.CategoryId,
                                CategoryName = category.CategoryName
                            }
                ).OrderByDescending(x => x.ProductCreatedAt).ToList();

            ViewBag.CategoryList = categoryList;

            return View(finalProduct);
            //return PartialView("_ProductList", finalProduct);
        }

        [HttpGet("GetProductByCategory/{categoryId}")]
        public async Task<IActionResult> GetProductByCategory(int categoryId)
        {
            var SelectedProductList = await context.Products.Include(x => x.Category).Where(x => x.CategoryId == categoryId).ToListAsync();
            if (SelectedProductList.Count == 0)
            {
                return BadRequest("No Result");
            }
            ViewBag.SelectedProductListByCategory = SelectedProductList;
            return View(SelectedProductList);
        }

        // Add
        [HttpGet("/AddProduct")]
        public async Task<IActionResult> Add()
        {
            // projection for dropdown
            var categoryList = await context.Categories.ToListAsync();

            //pass category list to view
            ViewBag.CategoryList = categoryList;
            return View();
        }

        [HttpPost("/AddProduct")]
        public async Task<IActionResult> Add(AddProductRequestDTO request)
        {
            var newProduct = new Product()
            {
                ProductId = Guid.NewGuid(),
                ProductName = request.ProductName,
                ProductDesc = request.ProductDesc,
                ProductPrice = request.ProductPrice,
                ProductCreatedAt = DateTime.Now,
                CategoryId = request.CategoryId,
                Category = request.Category
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
        [HttpGet("/UpdateProduct/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            var product = context.Products.FirstOrDefault(x => x.ProductId == id);
            if(product == null)
            {
                return NotFound();
            }
            var productList = new UpdateProductRequestDTO()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDesc = product.ProductDesc,
                ProductPrice = product.ProductPrice,
                CategoryId = product.CategoryId,
                Category = product.Category
            };
            ViewBag.CategoryList = await context.Categories.ToListAsync();

            return View(productList);
        }

        [HttpPost("/UpdateProduct/{id}")]
        public async Task<IActionResult> Update(UpdateProductRequestDTO request)
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
            existingProduct.CategoryId = request.CategoryId;
            existingProduct.Category = request.Category;

            await context.SaveChangesAsync();
            TempData["productsuccess"] = "Product Updated Successfully";

            return RedirectToAction("Index", "Product");
        }
    }
}
