namespace PairUpApi.Entity;

[Table("category")]
public class Category
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)] 
    public string Name { get; set; } = null!;
}