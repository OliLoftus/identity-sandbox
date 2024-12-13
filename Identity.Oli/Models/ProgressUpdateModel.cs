namespace Identity.Oli.Models;

public class ProgressUpdate
{
    public Guid Id { get; set; } // Unique identifier for the progress update
    public Guid GoalId { get; set; } // Links the progress update to a goal
    public DateTime Date { get; set; } // Date of the progress update
    public string Update { get; set; } // E.g., "Completed Chapter 1"
    public int PercentageComplete { get; set; } // E.g., 20% progress
}