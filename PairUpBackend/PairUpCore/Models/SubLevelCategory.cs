namespace PairUpCore.Models;

[Table("sub_level_category")]
public class SubLevelCategory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [StringLength(255)] 
    public string Name { get; set; } = null!;

    public ICollection<ActivitySubLevelCategory>? ActivitySubLevelCategories { get; set; }
}