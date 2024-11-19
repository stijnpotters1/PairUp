
namespace PairUpApi.Service;

public class ActivityService : IActivityService
{
    private readonly IMapper _mapper;

    public ActivityService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public ActivityResponse ConvertToResponse(Activity activity)
    {
        return _mapper.Map<ActivityResponse>(activity);
    }
}