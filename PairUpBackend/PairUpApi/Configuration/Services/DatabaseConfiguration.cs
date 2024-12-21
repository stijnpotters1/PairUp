namespace PairUpApi.Configuration.Services;

public static class DatabaseConfiguration
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, WebApplicationBuilder builder)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

        return services;
    }
}