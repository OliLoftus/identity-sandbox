namespace Identity.Oli.Models;

public class GoalModel
{
    public Guid Id { get; set; } // Unique identifier for the goal
    public string Title { get; set; } // E.g., "Learn React"
    public string Description { get; set; } // E.g., "Finish a React course by end of month"
    public string Category { get; set; } // E.g., "Career", "Fitness"
    public DateTime DueDate { get; set; } // Target completion date
    public bool IsCompleted { get; set; } // Status of the goal
    public List<ProgressUpdate> Progress { get; set; } = new(); // Progress updates associated with this goal
}