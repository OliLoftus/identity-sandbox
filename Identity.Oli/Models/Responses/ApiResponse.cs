namespace Identity.Oli.Models.Responses;

public class ApiResponse<T> // This is a generic class meaning it can wrap any type of data, this class wraps the response in a consistent shape.
{
    public string Status { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ApiResponse(string status, string message, T? data)
    {
        Status = status;
        Message = message;
        Data = data;
    }
}