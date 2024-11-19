namespace PairUpApi.Entity;

[Table("place")]
public class Place
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
    
    [StringLength(255)] 
    public string FullAddress { get; set; } = null!;

    public double Latitude { get; set; }
    
    public double Longitude { get; set; }

    public Activity Activity { get; set; } = new Activity();
}