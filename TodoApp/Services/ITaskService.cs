using TodoApp.ViewModels;
using Task = TodoApp.Models.Task;

namespace TodoApp.Services;

public interface ITaskService
{
    ResultViewModel<List<TaskViewModel>> GetAllTasks();
    ResultViewModel<TaskViewModel> GetTaskById(int id);
    ResultViewModel<TaskViewModel> CreateTask(TaskViewModel taskViewModel);
    ResultViewModel<TaskViewModel> UpdateTask(int id, TaskViewModel taskViewModel);
    ResultViewModel<bool> DeleteTask(int id);
}
