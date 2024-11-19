namespace PairUpApi.DTO.Responses;

public class ActivityResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public string Price { get; set; } = null!;
    public string Age { get; set; } = null!;
    public string Duration { get; set; } = null!;
    public CategoryResponse Category { get; set; } = new CategoryResponse();
    public PlaceResponse Place { get; set; } = new PlaceResponse();
}