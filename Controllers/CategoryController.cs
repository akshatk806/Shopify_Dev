using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product_Management.Data;
using Product_Management.Models.DomainModels;
using Product_Management.Models.DTO;

namespace Product_Management.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext context;
        public CategoryController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categoryList = await context.Categories.Where(x => x.CategoryId != 6).ToListAsync();
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
            var existingCategory = await context.Categories.FirstOrDefaultAsync(x => x.CategoryName.ToLower() == request.CategoryName.ToLower());
            if (existingCategory != null)
            {
                TempData["duplicateCategory"] = String.Format("Category {0} is Already Exists", request.CategoryName);
                return RedirectToAction("Add", "Category");
            }

            var newCategory = new Category()
            {
                CategoryName = request.CategoryName
            };

            await context.Categories.AddAsync(newCategory);
            await context.SaveChangesAsync();
            TempData["success"] = "Category Added Successfully";

            return RedirectToAction("Index", "Category");
        }
    }
}
