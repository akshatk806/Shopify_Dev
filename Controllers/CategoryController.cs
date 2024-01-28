using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;
        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categoryList = await context.Categories.ToListAsync();
            return View(categoryList);
        }

        // Add categories
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryRequestDTO request)
        {
            var newCategory = new Category()
            {
                CategoryName = request.CategoryName
            };

            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Category");
        }
    }
}
