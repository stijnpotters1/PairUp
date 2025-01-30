using PairUpCore.Exceptions.Activity;

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

        var filteredActivities = await query.ToListAsync();

        var activitiesWithDistanceFilter = FilterActivitiesByDistance(filteredActivities, requirements.Latitude, requirements.Longitude, requirements.Radius);

        var totalCount = activitiesWithDistanceFilter.Count;

        var paginatedActivities = ApplyPagination(activitiesWithDistanceFilter, requirements.PageNumber, requirements.PageSize);

        return new PagedActivityResponse<Activity>
        {
            Items = paginatedActivities,
            TotalCount = totalCount,
            PageNumber = requirements.PageNumber,
            PageSize = requirements.PageSize
        };
    }
    
    public async Task<Activity> AddLikeAsync(Guid userId, Guid activityId)
    {
        User? user = await _context.Users.Include(us => us.LikedActivities).FirstOrDefaultAsync(us => us.Id == userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        Activity? activity = await _context.Activities.FirstOrDefaultAsync(ac => ac.Id == activityId);
        if (activity == null)
        {
            throw new ActivityNotFoundException();
        }

        if (user.LikedActivities.Any(ac => ac.Id == activityId))
        {
            throw new ActivityIsAlreadyLikedByThisUserException();
        }
        
        user.LikedActivities.Add(activity);
        await _context.SaveChangesAsync();

        return activity;
    }

    public async Task<bool> RemoveLikeAsync(Guid userId, Guid activityId)
    {
        User? user = await _context.Users.Include(u => u.LikedActivities).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        Activity? activity = await _context.Activities.FirstOrDefaultAsync(ac => ac.Id == activityId);
        if (activity == null)
        {
            throw new ActivityNotFoundException();
        }

        if (user.LikedActivities.All(ac => ac.Id != activityId))
        {
            throw new ActivityIsNotLikedByThisUserException();
        }
        
        user.LikedActivities.Remove(activity);

        await _context.SaveChangesAsync();
        return true;
    }

    private IQueryable<Activity> BuildQuery(ActivityRequest requirements)
    {
        var query = _context.Activities
            .Include(a => a.ActivitySubLevelCategories)
            .ThenInclude(ac => ac.SubLevelCategory)
            .AsQueryable();

        query = ApplyCategoryFilter(query, requirements.TopLevelCategories, requirements.SubLevelCategories);

        return query;
    }


    private IQueryable<Activity> ApplyCategoryFilter(IQueryable<Activity> query, ICollection<TopLevelCategory> topLevelCategories, ICollection<string> subLevelCategories)
    {
        if (topLevelCategories.Any() || subLevelCategories.Any())
        {
            query = query.Where(a =>
                (topLevelCategories.Contains(a.TopLevelCategory) == true) ||
                (subLevelCategories.Any(sub => a.ActivitySubLevelCategories.Any(ac => ac.SubLevelCategory.Name == sub)) == true)
            );
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

    private List<Activity> ApplyPagination(List<Activity> activities, int pageNumber, int pageSize)
    {
        return activities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }
}