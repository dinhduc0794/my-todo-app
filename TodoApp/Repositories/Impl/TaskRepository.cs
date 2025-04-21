using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repositories.Interfaces;
using Task = TodoApp.Models.Task;

namespace TodoApp.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TodoAppDbContext _context;

    public TaskRepository(TodoAppDbContext context)
    {
        _context = context;
    }
    
// SELECT t.*, c.*
// FROM tasks t
// LEFT JOIN categories c ON t.CategoryId = c.CategoryId
// WHERE t.IsDeleted = 0
    public List<Task> GetAllTasks()
    {
        return _context.tasks
            .Include(t => t.Category)
            .Where(t => !t.IsDeleted)
            .ToList();
    }
    
// SELECT t.*, c.*
// FROM tasks t
// LEFT JOIN categories c ON t.CategoryId = c.CategoryId
// WHERE t.TaskId = @id AND t.IsDeleted = 0
    public Task GetTaskById(int id)
    {
        return _context.tasks
            .Include(t => t.Category)
            .FirstOrDefault(t => t.TaskId == id && !t.IsDeleted);
    }

    // INSERT INTO tasks (Title, Description, IsCompleted, DueDate, CategoryId, Priority, CreatedAt, UpdatedAt, IsActive, IsDeleted)
// VALUES (@Title, @Description, @IsCompleted, @DueDate, @CategoryId, @Priority, @CreatedAt, @UpdatedAt, @IsActive, @IsDeleted)
    public void AddTask(Task task)
    {
        _context.tasks.Add(task);
        _context.SaveChanges();
    }
    
// UPDATE tasks
    // SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted,
    //     DueDate = @DueDate, CategoryId = @CategoryId, Priority = @Priority, UpdatedAt = @UpdatedAt
    // WHERE TaskId = @TaskId AND IsDeleted = 0
    public void UpdateTask(Task task)
    {
        var existingTask = _context.tasks.Find(task.TaskId);
        if (existingTask == null || existingTask.IsDeleted)
        {
            throw new ArgumentException("Invalid task id");
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.IsCompleted = task.IsCompleted;
        existingTask.DueDate = task.DueDate;
        existingTask.CategoryId = task.CategoryId;
        existingTask.Priority = task.Priority;
        existingTask.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();
    }

    
// UPDATE tasks
// SET IsDeleted = 1, UpdatedAt = @UpdatedAt
// WHERE TaskId = @id AND IsDeleted = 0
    public bool DeleteTask(int id)
    {
        var task = _context.tasks.Find(id);
        if (task == null || task.IsDeleted)
        {
            return false;
        }

        task.IsDeleted = true;
        task.UpdatedAt = DateTime.UtcNow;
        _context.SaveChanges();
        return true;
    }
}