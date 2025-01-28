namespace PairUpCore.Models;

[Table("role")]
public class Role
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [StringLength(50)] 
    public string? Name { get; set; }

    public ICollection<User>? Users { get; set; } 
}