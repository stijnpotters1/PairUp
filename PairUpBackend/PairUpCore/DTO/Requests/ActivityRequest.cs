namespace PairUpCore.DTO.Requests;

public class ActivityRequest
{
    public ICollection<string>? Categories { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Radius { get; set; }

    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
}