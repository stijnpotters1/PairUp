namespace PairUpCore.Models;

[Table("activity_categories")]
public class ActivitySubLevelCategory
{
    public Guid Id { get; set; }
    public Guid ActivityId { get; set; }
    public Activity Activity { get; set; } = null!;
    public Guid SubLevelCategoryId { get; set; }
    public SubLevelCategory SubLevelCategory { get; set; } = null!;
}