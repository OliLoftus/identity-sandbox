using System.ComponentModel.DataAnnotations;
using Identity.Oli.Validation;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Oli.Models;

public class GoalModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Title { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    public string Category { get; set; }

    [Required]
    [FutureDate]
    public DateTime DueDate { get; set; }

    public bool IsCompleted { get; set; }

    public List<ProgressUpdate> Progress { get; set; } = new(); // List of progress updates
}