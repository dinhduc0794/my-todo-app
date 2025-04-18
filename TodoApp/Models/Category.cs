using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models;

public class Category : BaseModel
{
    [Key]
    public int CategoryId { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Task> Tasks { get; set; } = new List<Task>();
}