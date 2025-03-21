using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Oli.Models;

public class ProgressUpdate
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } // Unique identifier for the progress update
    public DateTime Date { get; set; } // Date of the progress update

    public string Update { get; set; } = string.Empty; //

    public int PercentageComplete { get; set; } // Progress percentage (0-100)
}