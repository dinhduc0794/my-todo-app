using System.ComponentModel.DataAnnotations;

namespace TodoApp.Enums
{
    public enum Priority
    {
        [Display(Name = "Low priority")]
        Low = 0,

        [Display(Name = "Medium priority")]
        Medium = 1,

        [Display(Name = "High priority")]
        High = 2,

        [Display(Name = "Critical")]
        Critical = 3
    }
}

