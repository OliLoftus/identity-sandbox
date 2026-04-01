namespace Identity.Oli.Models.Responses;

public class PagedResponse<T> : ApiResponse<List<T>>
{
    public int Page { get; }
    public int PageSize { get; }
    public long TotalCount { get; }
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

    public PagedResponse(
        string status,
        string message,
        List<T> data,
        int page,
        int pageSize,
        long totalCount)
        : base(status, message, data)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
    }
}
