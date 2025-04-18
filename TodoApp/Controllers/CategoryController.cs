using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;
using TodoApp.ViewModels;

namespace TodoApp.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    public IActionResult Index()
    {   
        var tasks = _categoryService.GetAllCategories();
        CategoryViewModel viewModel = new CategoryViewModel();
        return View(viewModel);
    }
    
    // GET: Category/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CategoryViewModel categoryViewModel)
    {
        if (ModelState.IsValid)
        {
            _categoryService.CreateCategory(categoryViewModel);
            return RedirectToAction(nameof(Index), "Home");
        }

        return View(categoryViewModel);
    }

    // GET: Category/Edit/5
    public IActionResult Edit(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    // POST: Category/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, CategoryViewModel categoryViewModel)
    {
        if (id != categoryViewModel.CategoryId)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var updated = _categoryService.UpdateCategory(id, categoryViewModel);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), "Home");
        }

        return View(categoryViewModel);
    }

    // POST: Category/Delete
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var deleted = _categoryService.DeleteCategory(id);
        return Json(new
        {
            success = deleted,
            message = deleted ? "Category deleted successfully" : "Category not found"
        });
    }

    // POST: Category/GetAll
    [HttpPost]
    public IActionResult GetAll(string keyword = "")
    {
        var categories = _categoryService.GetAllCategories()
            .Where(c => string.IsNullOrEmpty(keyword) || c.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            .Select(c => new
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            })
            .ToList();

        return Json(new { data = categories });
    }
}