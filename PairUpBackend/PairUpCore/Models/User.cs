namespace PairUpCore.Models;

[Table("user")]
public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)] public string? FirstName { get; set; }

    [StringLength(50)] public string? LastName { get; set; }

    [StringLength(50)] public string Email { get; set; } = null!;

    [StringLength(100)] public string Password { get; set; } = null!;

    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    
    public ICollection<Activity> LikedActivities { get; set; } = new HashSet<Activity>();
    
}