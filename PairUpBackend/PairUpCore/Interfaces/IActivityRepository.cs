namespace PairUpCore.Interfaces;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetActivitiesAsync(ActivityRequest requirements);
}