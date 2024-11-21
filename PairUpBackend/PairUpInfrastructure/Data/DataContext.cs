namespace PairUpInfrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityCategories> ActivityCategories { get; set; }
    public DbSet<Category> Categories { get; set; }
}