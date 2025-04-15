using TodoApp.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

public class Task : BaseModel
{
    [Key]
    public int TaskId { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? DueDate { get; set; }
    public int? CategoryId { get; set; }
    [ForeignKey("CategoryId")] public Category? Category { get; set; }
    public Priority? Priority { get; set; }
}