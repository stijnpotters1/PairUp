namespace PairUpCore.DTO.Requests;

public record UserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public Guid RoleId { get; set; }
}