namespace PairUpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityRepository _repository;
    private readonly IServices<Activity, ActivityResponse> _services;
    private readonly IService<Activity, ActivityResponse> _service;

    public ActivityController(IActivityRepository repository, IServices<Activity, ActivityResponse> services, IService<Activity, ActivityResponse> service)
    {
        _repository = repository;
        _services = services;
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<PagedActivityResponse<ActivityResponse>>> GetActivities(
        [FromBody] ActivityRequest requirements)
    {
        PagedActivityResponse<Activity> activities = await _repository.GetActivitiesAsync(requirements);

        var activityResponse = new PagedActivityResponse<ActivityResponse>
        {
            Items = _services.ConvertToResponse(activities.Items),
            TotalCount = activities.TotalCount,
            PageNumber = activities.PageNumber,
            PageSize = activities.PageSize
        };

        return Ok(activityResponse);
    }
    
    [HttpPost("like/{userId}/activity/{activityId}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<ActionResult<ActivityResponse>> LikeActivity(Guid userId, Guid activityId)
    {
        Activity likedActivity = await _repository.AddLikeAsync(userId, activityId);
        ActivityResponse likedActivityResponse = _service.ConvertToResponse(likedActivity);
        return Ok(likedActivityResponse);
    }

    [HttpDelete("unlike/{userId}/activity/{activityId}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<ActionResult<bool>> UnlikeActivity(Guid userId, Guid activityId)
    {
        var response = await _repository.RemoveLikeAsync(userId, activityId);
        return Ok(response);
    }
}