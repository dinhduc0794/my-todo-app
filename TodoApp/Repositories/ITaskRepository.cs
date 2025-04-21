using TodoApp.Models;
using Task = TodoApp.Models.Task;

namespace TodoApp.Repositories.Interfaces;

public interface ITaskRepository
{
    List<Task> GetAllTasks();
    Task GetTaskById(int id);
    void AddTask(Task task);
    void UpdateTask(Task task);
    bool DeleteTask(int id);
}