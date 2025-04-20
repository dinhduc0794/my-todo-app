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
        List<TaskViewModel> tasks = _taskService.GetAllTasks();
        TaskViewModel viewModel = new TaskViewModel();
        viewModel.RecordCount = tasks.Count;
        return View(viewModel);
    }
    
// GET: Task
    [HttpGet]
    public IActionResult GetAllTasks()
    {
        List<TaskViewModel> tasks = _taskService.GetAllTasks();
        return Json(new { data = tasks });
    }

// GET: Task/Details/5
    public IActionResult Details(int id)
    {
        TaskViewModel task = _taskService.GetTaskById(id);
        if (task == null)
        {
            return NotFound();
        }
    
        return View(task);
    }   

// GET: Task/Create
    public IActionResult Form(int id)
    {      
        TaskViewModel viewModel = new TaskViewModel();
        viewModel.IsEdit = id > 0;
        if (viewModel.IsEdit)
        {
            var task = _taskService.GetTaskById(id);
            if (task == null)
            {
                return NotFound();
            }
            viewModel = task;
        }
        viewModel.AllCategories = _categoryService.GetAllCategories();
        return View(viewModel);
    }
    
// POST: Task/Create
    [HttpPost]
    public IActionResult Create(TaskViewModel taskViewModel)
    {
        TaskViewModel createdTask = new TaskViewModel();
       
        createdTask = _taskService.CreateTask(taskViewModel);
        
        if (createdTask != null) {
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ModelState.AddModelError("", "Failed to create task.");
        }
        return Json(new { success = false, message = "Failed to create category." });
    }
    

// POST: Task/Edit/5
    [HttpPost]
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