using Microsoft.AspNetCore.Mvc;
using TodoApp.Services;
using TodoApp.ViewModels;

namespace TodoApp.Controllers;

public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly ICategoryService _categoryService;

    public TaskController(ITaskService taskService, ICategoryService categoryService)
    {
        _taskService = taskService;
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        var result = _taskService.GetAllTasks();
        if (!result.IsSuccess)
        {
            ViewBag.ErrorMessage = result.Message;
            return View(new TaskViewModel());
        }

        var viewModel = new TaskViewModel
        {
            RecordCount = result.Data?.Count ?? 0
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult GetAllTasks()
    {
        var result = _taskService.GetAllTasks();
        if (!result.IsSuccess)
        {
            return Json(new { success = false, message = result.Message });
        }
        return Json(new { data = result.Data });
    }

    public IActionResult Details(int id)
    {
        var result = _taskService.GetTaskById(id);
        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    public IActionResult Form(int id)
    {
        TaskViewModel viewModel = new TaskViewModel { IsEdit = id > 0 };

        if (viewModel.IsEdit)
        {
            var result = _taskService.GetTaskById(id);
            if (!result.IsSuccess || result.Data == null)
            {
                return NotFound();
            }
            viewModel = result.Data;
        }

        var categoryResult = _categoryService.GetAllCategories();
        if (categoryResult.IsSuccess)
        {
            viewModel.AllCategories = categoryResult.Data;
        }
        else
        {
            ModelState.AddModelError("", categoryResult.Message);
        }

        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(TaskViewModel taskViewModel)
    {
        var result = _taskService.CreateTask(taskViewModel);
        if (result.IsSuccess)
        {
            return RedirectToAction(nameof(Index));
        }

        taskViewModel.AllCategories = _categoryService.GetAllCategories().Data;
        return Json(new { success = false, message = result.Message });
    }

    [HttpPost]
    public IActionResult Edit(TaskViewModel taskViewModel)
    {
        if (taskViewModel.TaskId == 0)
        {
            return BadRequest();
        }

        var result = _taskService.UpdateTask(taskViewModel.TaskId, taskViewModel);
        if (result.IsSuccess && result.Data != null)
        {
            return RedirectToAction(nameof(Index));
        }

        taskViewModel.AllCategories = _categoryService.GetAllCategories().Data;
        return Json(new { success = false, message = result.Message });
    }

    public IActionResult Delete(int id)
    {
        var result = _taskService.GetTaskById(id);
        if (!result.IsSuccess || result.Data == null)
        {
            return NotFound();
        }

        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var result = _taskService.DeleteTask(id);
        if (!result.IsSuccess || result.Data == false)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}