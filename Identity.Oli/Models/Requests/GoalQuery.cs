namespace Identity.Oli.Models.Requests;

public class GoalQuery
{
    public string? Category { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DueFrom { get; set; }
    public DateTime? DueTo { get; set; }
    public string? TitleContains { get; set; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    public string? SortBy { get; set; }
    public string? SortDir { get; set; }
}
