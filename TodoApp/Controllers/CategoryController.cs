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
        CategoryViewModel viewModel = new CategoryViewModel();
        return View(viewModel);
    }
    
    [HttpGet]
    public IActionResult GetAllCategories()
    {
        List<CategoryViewModel> tasks = _categoryService.GetAllCategories();
        return Json(new { data = tasks });
    }
    
    public IActionResult Details(int id)
    {
        CategoryViewModel category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }
    
        return View(category);
    }   
    
    // GET: Task/Create
    public IActionResult Form(int id)
    {      
        CategoryViewModel viewModel = new CategoryViewModel();
        viewModel.IsEdit = id > 0;
        if (viewModel.IsEdit)
        {
            CategoryViewModel categoryViewModel = _categoryService.GetCategoryById(id);
            if (categoryViewModel == null)
            {
                return NotFound();
            }
            viewModel = categoryViewModel;
        }
        return View(viewModel);
    }
    
// POST: Task/Create
    [HttpPost]
    public IActionResult Create(CategoryViewModel categoryViewModel)
    {
        CategoryViewModel createdCategory = _categoryService.CreateCategory(categoryViewModel);
        
        if (createdCategory != null) {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError("", "Failed to create category.");
        }
        return Json(new { success = false, message = "Failed to create category." });
    }

    
    [HttpPost]
    public IActionResult Edit(CategoryViewModel categoryViewModel)
    {  
        if (categoryViewModel.CategoryId == 0)
        {
            return BadRequest();
        }
        
        if (ModelState.IsValid)
        {
            var updated = _categoryService.UpdateCategory(categoryViewModel.CategoryId, categoryViewModel);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        return Json(categoryViewModel);
    }


    // GET: Category/Delete/5
    public IActionResult Delete(int id)
    {
        CategoryViewModel category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    
    // POST: Category/Delete/5  
    [HttpPost, ActionName("Delete")]
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