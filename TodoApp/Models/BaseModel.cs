using System.ComponentModel;

namespace TodoApp.Models;

public abstract class BaseModel
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
    
    [DefaultValue(true)]
    public bool IsActive { get; set; } = true;
    
    [DefaultValue(false)]
    public bool IsDeleted { get; set; } = false;    
}