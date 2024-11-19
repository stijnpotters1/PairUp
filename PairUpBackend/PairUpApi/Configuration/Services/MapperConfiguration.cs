namespace PairUpApi.Configuration.Services;

public static class MappingConfiguration
{
    public static IServiceCollection ConfigureMapping(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        return services;
    }
}