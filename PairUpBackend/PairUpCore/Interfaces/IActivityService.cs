namespace PairUpCore.Interfaces;

public interface IActivityService
{
    IEnumerable<ActivityResponse> ConvertToResponse(IEnumerable<Activity> activities);
}