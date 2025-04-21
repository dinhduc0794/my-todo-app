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

    public ResultViewModel<List<TaskViewModel>> GetAllTasks()
    {
        var tasks = _taskRepository.GetAllTasks();

        var result = tasks.Select(t => new TaskViewModel
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

        return ResultViewModel<List<TaskViewModel>>.Success(result, "Lấy danh sách công việc thành công");
    }

    public ResultViewModel<TaskViewModel> GetTaskById(int id)
    {
        var task = _taskRepository.GetTaskById(id);
        if (task == null)
        {
            return ResultViewModel<TaskViewModel>.Failure("Không tìm thấy công việc");
        }

        var taskViewModel = new TaskViewModel
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

        return ResultViewModel<TaskViewModel>.Success(taskViewModel, "Lấy danh sách công việc thành công");
    }

    public ResultViewModel<TaskViewModel> CreateTask(TaskViewModel taskViewModel)
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

        return ResultViewModel<TaskViewModel>.Success(taskViewModel, "Tạo mới công việc thành công");
    }

    public ResultViewModel<TaskViewModel> UpdateTask(int id, TaskViewModel taskViewModel)
    {
        if (id != taskViewModel.TaskId)
        {
            return ResultViewModel<TaskViewModel>.Failure("ID không hợp lệ");
        }

        var existing = _taskRepository.GetTaskById(id);
        if (existing == null)
        {
            return ResultViewModel<TaskViewModel>.Failure("Không tìm thấy công việc");
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
            UpdatedAt = DateTime.UtcNow,
            IsActive = taskViewModel.IsActive,
            IsDeleted = taskViewModel.IsDeleted
        };

        _taskRepository.UpdateTask(task);

        taskViewModel.UpdatedAt = task.UpdatedAt;

        return ResultViewModel<TaskViewModel>.Success(taskViewModel, "Cập nhật công việc thành công");
    }

    public ResultViewModel<bool> DeleteTask(int id)
    {
        var success = _taskRepository.DeleteTask(id);
        if (!success)
        {
            return ResultViewModel<bool>.Failure("Xóa công việc thất bại");
        }

        return ResultViewModel<bool>.Success(true, "Xóa công việc thành công");
    }

}
