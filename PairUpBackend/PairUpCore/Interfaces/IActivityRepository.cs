namespace PairUpCore.Interfaces;

public interface IActivityRepository
{
    Task<PagedActivityResponse<Activity>> GetActivitiesAsync(ActivityRequest requirements);
}