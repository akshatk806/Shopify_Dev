using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models;
using Product_Management.Models.DomainModels;
using System.Diagnostics;

namespace Product_Management.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var allProducts = _context.Products.Include(x => x.Category).Where(x => x.IsActive == true).OrderByDescending(x => x.ProductCreatedAt).ToList();
            ViewBag.trendingProducts = allProducts.Where(x => x.IsTrending == true).Where(x => x.IsActive == true).OrderByDescending(x => x.ProductCreatedAt).ToList();
            ViewBag.CategoryList = _context.Categories.ToList();
            return View(allProducts);
        }


        public IActionResult GetProductsByCategory(int categoryId)
        {
            if(categoryId == 6)
            {
                var allProducts = _context.Products.Include(x => x.Category).Where(x => x.IsActive == true).OrderByDescending(x => x.ProductCreatedAt).ToList();
                return PartialView("_ProductByCategoryPartial", allProducts);
            }
            var productsByCategory = _context.Products.Include(x => x.Category).Where(x => x.CategoryId == categoryId && x.IsActive == true).OrderByDescending(x => x.ProductCreatedAt).ToList();

            return PartialView("_ProductByCategoryPartial", productsByCategory);
        }

        [HttpPost]
        public IActionResult Privacy(string myData)
        {
            //var mycName = JsonConvert.SerializeObject<Product>(cName);
            int CategoryId = _context.Categories.FirstOrDefault(x=>x.CategoryName == myData).CategoryId;
            TempData["CategoryId"] = CategoryId;
            return Json(new { result = "success" });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
