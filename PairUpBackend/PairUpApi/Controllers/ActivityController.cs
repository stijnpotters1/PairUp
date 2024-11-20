namespace PairUpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ActivityController : ControllerBase
{
    private readonly IActivityRepository _repository;
    private readonly IActivityService _service;

    public ActivityController(IActivityRepository repository, IActivityService service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<PagedActivityResponse<ActivityResponse>>> GetActivities(
        [FromBody] ActivityRequest requirements)
    {
        PagedActivityResponse<Activity> activities = await _repository.GetActivitiesAsync(requirements);

        var activityResponse = new PagedActivityResponse<ActivityResponse>
        {
            Items = _service.ConvertToResponse(activities.Items),
            TotalCount = activities.TotalCount,
            PageNumber = activities.PageNumber,
            PageSize = activities.PageSize
        };

        return Ok(activityResponse);
    }
}