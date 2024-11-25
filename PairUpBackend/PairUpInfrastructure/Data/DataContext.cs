namespace PairUpInfrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivitySubLevelCategory> ActivitySubLevelCategories { get; set; }
    public DbSet<SubLevelCategory> SubLevelCategories { get; set; }
}