using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }

    public List<Task> Tasks { get; set; } = new List<Task>();
}