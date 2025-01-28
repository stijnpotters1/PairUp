namespace PairUpCore.DTO.Responses;

public record SubLevelCategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}