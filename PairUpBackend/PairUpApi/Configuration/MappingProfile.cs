namespace PairUpApi.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Activity, ActivityResponse>()
            .ForMember(dest => dest.SubLevelCategories, opt => opt.MapFrom(src => src.ActivitySubLevelCategories.Select(asc => asc.SubLevelCategory)));

        CreateMap<SubLevelCategory, SubLevelCategoryResponse>();
    }
}