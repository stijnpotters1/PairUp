namespace PairUpCore.DTO.Responses;

public class PagedActivityResponse<T>
{
    public IEnumerable<T> Items { get; set; } = null!;
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}