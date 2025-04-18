using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repositories;
using TodoApp.Repositories.Interfaces;
using TodoApp.ViewModels;
using Task = TodoApp.Models.Task;

namespace TodoApp.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ICategoryRepository _categoryRepository;

    public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository)
    {
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
    }

    public List<TaskViewModel> GetAllTasks()
    {
        var tasks = _taskRepository.GetAllTasks();
        return tasks.Select(t => new TaskViewModel
        {
            TaskId = t.TaskId,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            DueDate = t.DueDate,
            CategoryId = t.CategoryId,
            Priority = t.Priority,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            IsActive = t.IsActive,
            IsDeleted = t.IsDeleted,
            Category = t.Category != null
                ? new CategoryViewModel
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description,
                    CreatedAt = t.Category.CreatedAt,
                    UpdatedAt = t.Category.UpdatedAt,
                    IsActive = t.Category.IsActive,
                    IsDeleted = t.Category.IsDeleted
                }
                : null
        }).ToList();
    }

    public TaskViewModel GetTaskById(int id)
    {
        var task = _taskRepository.GetTaskById(id);
        if (task == null)
        {
            return null;
        }

        return new TaskViewModel
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            DueDate = task.DueDate,
            CategoryId = task.CategoryId,
            Priority = task.Priority,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            IsActive = task.IsActive,
            IsDeleted = task.IsDeleted,
            Category = task.Category != null
                ? new CategoryViewModel
                {
                    CategoryId = task.Category.CategoryId,
                    Name = task.Category.Name,
                    Description = task.Category.Description,
                    CreatedAt = task.Category.CreatedAt,
                    UpdatedAt = task.Category.UpdatedAt,
                    IsActive = task.Category.IsActive,
                    IsDeleted = task.Category.IsDeleted
                }
                : null
        };
    }

    public TaskViewModel CreateTask(TaskViewModel taskViewModel)
    {
        var task = new Task
        {
            Title = taskViewModel.Title,
            Description = taskViewModel.Description,
            IsCompleted = taskViewModel.IsCompleted,
            DueDate = taskViewModel.DueDate,
            CategoryId = taskViewModel.CategoryId,
            Priority = taskViewModel.Priority,
            CreatedAt = DateTime.UtcNow,
            IsActive = true,
            IsDeleted = false
        };

        _taskRepository.AddTask(task);

        taskViewModel.TaskId = task.TaskId;
        taskViewModel.CreatedAt = task.CreatedAt;
        taskViewModel.IsActive = task.IsActive;
        taskViewModel.IsDeleted = task.IsDeleted;

        return taskViewModel;
    }

    public bool UpdateTask(int id, TaskViewModel taskViewModel)
    {
        if (id != taskViewModel.TaskId)
        {
            return false;
        }

        var task = new Task
        {
            TaskId = taskViewModel.TaskId,
            Title = taskViewModel.Title,
            Description = taskViewModel.Description,
            IsCompleted = taskViewModel.IsCompleted,
            DueDate = taskViewModel.DueDate,
            CategoryId = taskViewModel.CategoryId,
            Priority = taskViewModel.Priority,
            CreatedAt = taskViewModel.CreatedAt,
            IsActive = taskViewModel.IsActive,
            IsDeleted = taskViewModel.IsDeleted
        };

        return _taskRepository.UpdateTask(task);
    }

    public bool DeleteTask(int id)
    {
        return _taskRepository.DeleteTask(id);
    }

    public List<CategoryViewModel> GetAllCategories()
    {
        var categories = _categoryRepository.GetAllCategories();
        return categories.Select(c => new CategoryViewModel
        {
            CategoryId = c.CategoryId,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt,
            IsActive = c.IsActive,
            IsDeleted = c.IsDeleted
        }).ToList();
    }
}