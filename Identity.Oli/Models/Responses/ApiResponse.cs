namespace Identity.Oli.Models.Responses;

public class ApiResponse<T>
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