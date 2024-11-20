namespace PairUpCore.DTO.Requests;

public class ActivityRequest
{
    public ICollection<string>? Categories { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Radius { get; set; }
}