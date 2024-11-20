namespace PairUpCore.Models;

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

    [StringLength(255)]
    public string Duration { get; set; } = null!;
    
    [StringLength(255)]
    public string FullAddress { get; set; } = null!;
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public Guid CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}