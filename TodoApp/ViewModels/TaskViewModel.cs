using Task = TodoApp.Models.Task;

namespace TodoApp.ViewModels;

public class TaskViewModel : Task
{
    public CategoryViewModel Category { get; set; }
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