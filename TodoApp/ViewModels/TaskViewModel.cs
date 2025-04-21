using Task = TodoApp.Models.Task;

namespace TodoApp.ViewModels;

public class TaskViewModel : Task
{
    public CategoryViewModel Category { get; set; }
    public int RecordCount { get; set; }
    public List<CategoryViewModel> AllCategories { get; set; }
    public bool IsEdit { get; set; }
    public bool IsSuccess { get; set; }
}


/*
 *     [Key]
    public int TaskId { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? DueDate { get; set; }
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category? Category { get; set; }
    public Priority? Priority { get; set; }
*/