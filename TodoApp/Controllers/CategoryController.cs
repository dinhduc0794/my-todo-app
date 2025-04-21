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
        var result = _categoryService.GetAllCategories();
        if (!result.IsSuccess)
        {
            ViewBag.ErrorMessage = result.Message;
            return View();
        }

        var viewModel = new CategoryViewModel
        {
            RecordCount = result.Data?.Count ?? 0
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var result = _categoryService.GetAllCategories();
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { data = result.Data });
    }

    public IActionResult Details(int id)
    {
        var result = _categoryService.GetCategoryById(id);
        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    public IActionResult Form(int id)
    {
        CategoryViewModel viewModel = new CategoryViewModel { IsEdit = id > 0 };

        if (viewModel.IsEdit)
        {
            var result = _categoryService.GetCategoryById(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }
            viewModel = result.Data;
        }

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(CategoryViewModel categoryViewModel)
    {
        var result = _categoryService.CreateCategory(categoryViewModel);

        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result.Message);
        return Json(new { success = false, message = result.Message });
    }

    [HttpPost]
    public IActionResult Edit(CategoryViewModel categoryViewModel)
    {
        if (categoryViewModel.CategoryId == 0)
        {
            return BadRequest();
        }

        var result = _categoryService.UpdateCategory(categoryViewModel.CategoryId, categoryViewModel);
        if (result.IsSuccess && result.Data != null)
        {
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", result.Message);
        return Json(new { success = false, message = result.Message });
    }

    public IActionResult Delete(int id)
    {
        var result = _categoryService.GetCategoryById(id);
        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var result = _categoryService.DeleteCategory(id);
        return Json(result);
        if (!result.IsSuccess || result.Data == false)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}