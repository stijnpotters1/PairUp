namespace PairUpCore.DTO.Requests;

public record ActivityRequest
{
    public ICollection<TopLevelCategory> TopLevelCategories { get; set; }
    
    public ICollection<string>? SubLevelCategories { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Radius { get; set; }

    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
}