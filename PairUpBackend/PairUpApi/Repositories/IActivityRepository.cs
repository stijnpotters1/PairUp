namespace PairUpApi.Repositories;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetActivitiesAsync(ActivityRequest requirements);
}