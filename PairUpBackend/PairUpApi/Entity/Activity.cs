namespace PairUpApi.Entity;

[Table("activity")]
public class Activity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)] 
    public string Name { get; set; } = null!;

    [StringLength(255)] 
    public string Description { get; set; } = null!;

    [StringLength(255)] 
    public string Url { get; set; } = null!;

    [StringLength(255)] 
    public string Price { get; set; } = null!;

    [StringLength(255)]
    public string Age { get; set; } = null!;

    public string Duration { get; set; } = null!;
    
    public Guid PlaceId { get; set; }

    public Place Place { get; set; } = null!;
}