using System.ComponentModel.DataAnnotations;
using Identity.Oli.Validation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Oli.Models;

public class GoalModel
{
    // private constructor ensure objects can only be created by factory method
    private GoalModel()
    {
        Title = string.Empty;
        Description = string.Empty;
        Category = string.Empty;
        Progress = new List<ProgressUpdate>();
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; private set; } // prevents external modification

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; private set; }

    [StringLength(500)]
    public string Description { get; private set; }

    [Required]
    public string Category { get; private set; }

    [Required]
    [FutureDate]
    public DateTime DueDate { get; private set; }

    public bool IsCompleted { get; private set; }

    public List<ProgressUpdate> Progress { get; private set; } = new(); // List of progress updates

    // Factory method for creating a new GoalModel instance
    // Ensure proper initialization
    public static GoalModel Create(string title, string description, string category, DateTime dueDate,
        bool isCompleted)
    {
        return new GoalModel
        {
            Id = Guid.NewGuid(), // always generates a unique ID
            Title = title,
            Description = description,
            Category = category,
            DueDate = dueDate,
            IsCompleted = isCompleted
        };
    }

    public void Update(string title, string description, string category, DateTime dueDate, bool isCompleted)
    {
        Title = title;
        Description = description;
        Category = category;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }
}