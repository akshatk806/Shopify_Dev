using Microsoft.AspNetCore.Mvc;

namespace Product_Management.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
