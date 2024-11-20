namespace PairUpCore.DTO.Responses;

public class PlaceResponse
{
    public Guid Id { get; set; }
    public string FullAddress { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}