namespace PairUpCore.Interfaces;

public interface IActivityRepository
{
    Task<PagedActivityResponse<Activity>> GetActivitiesAsync(ActivityRequest requirements);
    Task<Activity> AddLikeAsync(Guid userId, Guid activityId);
    Task<bool> RemoveLikeAsync(Guid userId, Guid activityId); 
}