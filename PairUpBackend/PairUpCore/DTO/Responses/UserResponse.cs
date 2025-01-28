namespace PairUpCore.DTO.Responses;

public record UserResponse
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public RoleDto Role { get; set; } = null!;
    public ICollection<ActivityResponse> LikedActivities { get; set; } = new HashSet<ActivityResponse>();
}