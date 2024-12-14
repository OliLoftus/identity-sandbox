using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Oli.Models;

public class ProgressUpdate
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } // Unique identifier for the progress update

    public DateTime Date { get; set; } // Date of the progress update

    [StringLength(500)]
    public string Update { get; set; } // Description of the progress

    [Range(0, 100)]
    public int PercentageComplete { get; set; } // Progress percentage (0-100)
}