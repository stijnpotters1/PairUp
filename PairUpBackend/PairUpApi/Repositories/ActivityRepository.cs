namespace PairUpApi.Repositories;

public class ActivityRepository : IActivityRepository
{
    private readonly DataContext _context;

    public ActivityRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Activity>> GetActivitiesAsync(ActivityRequest requirements)
    {
        return await _context.Activities
            .Include(a => a.Category)
            .Include(a => a.Place)
            .ToListAsync();
    }

    private Task CalculateCoordinatesBasedOnUserLocation()
    {
        throw new NotImplementedException();
    }
}