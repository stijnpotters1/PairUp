namespace PairUpApi.Service;

public class ActivityService : IService<Activity, ActivityResponse>
{
    private readonly IMapper _mapper;

    public ActivityService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    private ActivityResponse ConvertToResponse(Activity activity)
    {
        return _mapper.Map<ActivityResponse>(activity);
    }

    public IEnumerable<ActivityResponse> ConvertToResponse(IEnumerable<Activity> activities)
    {
        return activities.Select(ConvertToResponse);
    }
}