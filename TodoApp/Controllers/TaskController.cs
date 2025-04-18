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

// GET: Task
    public IActionResult Index()
    {
        var tasks = _taskService.GetAllTasks();
        return View(tasks);
    }

// GET: Task/Details/5
    public IActionResult Details(int id)
    {
        var task = _taskService.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

// GET: Task/Create
    public IActionResult Create()
    {
        ViewBag.Categories = _categoryService.GetAllCategories();
        return View();
    }

// POST: Task/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(TaskViewModel taskViewModel)
    {
        if (ModelState.IsValid)
        {
            _taskService.CreateTask(taskViewModel);
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetAllCategories();
        return View(taskViewModel);
    }

// GET: Task/Edit/5
    public IActionResult Edit(int id)
    {
        var task = _taskService.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }

        ViewBag.Categories = _categoryService.GetAllCategories();
        return View(task);
    }

// POST: Task/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, TaskViewModel taskViewModel)
    {
        if (id != taskViewModel.TaskId)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var updated = _taskService.UpdateTask(id, taskViewModel);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Categories = _categoryService.GetAllCategories();
        return View(taskViewModel);
    }

// GET: Task/Delete/5
    public IActionResult Delete(int id)
    {
        var task = _taskService.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

// POST: Task/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var deleted = _taskService.DeleteTask(id);
        if (!deleted)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}