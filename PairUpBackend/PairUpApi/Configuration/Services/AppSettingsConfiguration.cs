namespace PairUpApi.Configuration.Services;

public static class AppSettingsConfiguration
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
        Env.Load();

        builder.Configuration["ConnectionStrings:DatabaseConnection"] = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        return services;
    }
}