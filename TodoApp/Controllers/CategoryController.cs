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

// GET: Category
    public IActionResult Index()
    {
        var categories = _categoryService.GetAllCategories();
        return View(categories);
    }

// GET: Category/Details/5
    public IActionResult Details(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
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
            return RedirectToAction(nameof(Index));
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
    public IActionResult Edit(int id, CategoryViewModel
        categoryViewModel)
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

            return RedirectToAction(nameof(Index));
        }

        return View(categoryViewModel);
    }

// GET: Category/Delete/5
    public IActionResult Delete(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

// POST: Category/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var deleted = _categoryService.DeleteCategory(id);
        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}