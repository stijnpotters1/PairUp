namespace PairUpInfrastructure.Repositories;

public class ActivityRepository : IActivityRepository
{
    private const double EarthRadiusKm = 6371.0;
    private readonly DataContext _context;

    public ActivityRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<PagedActivityResponse<Activity>> GetActivitiesAsync(ActivityRequest requirements)
    {
        var query = BuildQuery(requirements);

        var totalCount = await GetTotalCount(query);

        var activities = await ApplyPagination(query, requirements);

        var filteredActivities = FilterActivitiesByDistance(activities, requirements.Latitude, requirements.Longitude, requirements.Radius);

        return new PagedActivityResponse<Activity>
        {
            Items = filteredActivities,
            TotalCount = totalCount,
            PageNumber = requirements.PageNumber,
            PageSize = requirements.PageSize
        };
    }

    private IQueryable<Activity> BuildQuery(ActivityRequest requirements)
    {
        var query = _context.Activities
            .Include(a => a.ActivitySubLevelCategories)
            .ThenInclude(ac => ac.SubLevelCategory)
            .AsQueryable();

        query = ApplyCategoryFilter(query, requirements.SubLevelCategories);

        return query;
    }


    private IQueryable<Activity> ApplyCategoryFilter(IQueryable<Activity> query, ICollection<string>? categories)
    {
        if (categories?.Any() == true)
        {
            query = query.Where(a => a.ActivitySubLevelCategories
                .Any(ac => categories.Contains(ac.SubLevelCategory.Name)));
        }

        return query;
    }

    private List<Activity> FilterActivitiesByDistance(List<Activity> activities, double userLatitude, double userLongitude, int radius)
    {
        return activities.Where(a =>
            CalculateDistance(userLatitude, userLongitude, a.Latitude, a.Longitude) <= radius).ToList();
    }

    private double CalculateDistance(double userLatitude, double userLongitude, double activityLatitude, double activityLongitude)
    {
        var lat1 = Math.PI / 180 * userLatitude;
        var lon1 = Math.PI / 180 * userLongitude;
        var lat2 = Math.PI / 180 * activityLatitude;
        var lon2 = Math.PI / 180 * activityLongitude;

        return EarthRadiusKm * Math.Acos(
            Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1) + Math.Sin(lat1) * Math.Sin(lat2)
        );
    }

    private async Task<int> GetTotalCount(IQueryable<Activity> query)
    {
        return await query.CountAsync();
    }

    private async Task<List<Activity>> ApplyPagination(IQueryable<Activity> query, ActivityRequest requirements)
    {
        return await query
            .Skip((requirements.PageNumber - 1) * requirements.PageSize)
            .Take(requirements.PageSize)
            .ToListAsync();
    }
}