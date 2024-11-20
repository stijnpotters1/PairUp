namespace PairUpInfrastructure.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Place> Places { get; set; }
}