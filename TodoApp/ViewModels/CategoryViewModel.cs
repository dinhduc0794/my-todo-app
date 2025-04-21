using TodoApp.Models;

namespace TodoApp.ViewModels;

public class CategoryViewModel : Category
{
    public bool IsEdit { get; set; }
    public bool IsSuccess { get; set; }
    public int RecordCount { get; set; }
}               