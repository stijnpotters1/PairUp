namespace PairUpApi.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Activity, ActivityResponse>();
    }
}