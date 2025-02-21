using System.ComponentModel.DataAnnotations;

namespace Identity.Oli.Models;

public class GoalRequest
{
    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }
}