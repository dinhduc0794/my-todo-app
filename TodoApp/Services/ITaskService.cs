using TodoApp.ViewModels;
using Task = TodoApp.Models.Task;

namespace TodoApp.Services;

public interface ITaskService
{
    List<TaskViewModel> GetAllTasks();
    TaskViewModel GetTaskById(int id);
    TaskViewModel CreateTask(TaskViewModel taskViewModel);
    bool UpdateTask(int id, TaskViewModel taskViewModel);
    bool DeleteTask(int id);
}   