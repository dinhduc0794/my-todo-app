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

    public List<Task> GetAllTasks()
    {
        return _context.tasks
            .Include(t => t.Category)
            .Where(t => !t.IsDeleted)
            .ToList();
    }

    public Task GetTaskById(int id)
    {
        return _context.tasks
            .Include(t => t.Category)
            .FirstOrDefault(t => t.TaskId == id && !t.IsDeleted);
    }

    public void AddTask(Task task)
    {
        _context.tasks.Add(task);
        _context.SaveChanges();
    }

    public bool UpdateTask(Task task)
    {
        var existingTask = _context.tasks.Find(task.TaskId);
        if (existingTask == null || existingTask.IsDeleted)
        {
            return false;
        }

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.IsCompleted = task.IsCompleted;
        existingTask.DueDate = task.DueDate;
        existingTask.CategoryId = task.CategoryId;
        existingTask.Priority = task.Priority;
        existingTask.UpdatedAt = DateTime.UtcNow;

        try
        {
            _context.SaveChanges();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

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