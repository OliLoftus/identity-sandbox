namespace Identity.Oli.Models.Responses;

public class ErrorResponse
{
    public string Status { get; set; }
    public string Message { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ErrorResponse(string status, string message, IDictionary<string, string[]>? errors = null)
    {
        Status = status;
        Message = message;
        Errors = errors;
    }
}
