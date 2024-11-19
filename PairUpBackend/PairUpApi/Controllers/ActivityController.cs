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
    public async Task<ActionResult<IEnumerable<ActivityResponse>>> GetActivities(
        [FromBody] ActivityRequest requirements)
    {
        IEnumerable<Activity> activities = await _repository.GetActivitiesAsync(requirements);
        IEnumerable<ActivityResponse> activityResponses = activities.Select(ac => _service.ConvertToResponse(ac));
        return Ok(activityResponses);
    }
}