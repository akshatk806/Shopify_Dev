using Microsoft.AspNetCore.Mvc;
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
            return View();
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
