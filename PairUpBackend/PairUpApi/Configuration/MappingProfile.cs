namespace PairUpApi.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
            .ForMember(dest => dest.LikedActivities, opt => opt.MapFrom(src => src.LikedActivities));
        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        CreateMap<User, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Password, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        
        CreateMap<Role, RoleDto>();
        
        CreateMap<Activity, ActivityResponse>()
            .ForMember(dest => dest.SubLevelCategories, opt => opt.MapFrom(src => src.ActivitySubLevelCategories.Select(asc => asc.SubLevelCategory)));

        CreateMap<SubLevelCategory, SubLevelCategoryResponse>();
    }
}