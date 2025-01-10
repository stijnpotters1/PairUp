namespace PairUpApi.Configuration;

public static class ServiceContainer
{
    public static IServiceCollection InstantiateServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        // Configure app settings
        services.ConfigureAppSettings(builder);
        
        // CORS Configuration
        services.ConfigureCustomCors();

        // Add controllers
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        // Add httpclient
        services.AddHttpClient();
        
        // Add swagger configuration 
        services.AddSwaggerGen();

        // Configure HTTPS redirection
        services.AddHttpsRedirection(options => { options.HttpsPort = 443; });

        // Database Configuration
        services.ConfigureDatabase(builder);
        
        // Automapper Configuration
        services.ConfigureMapping();

        // Scoped custom services (Dependency injection for services and repositories)
        services.AddScoped<IService<Activity, ActivityResponse>, ActivityService>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IService<SubLevelCategory, SubLevelCategoryResponse>, SubLevelCategoryService>();
        services.AddScoped<ISubLevelCategoryRepository, SubLevelCategoryRepository>();

        services.AddSingleton<HttpClient>();
        
        // Scraper registration
        services.AddScoped<BaseWebScraper, BijzonderPlekjeAccommodationScraper>();
        services.AddScoped<BaseWebScraper, BijzonderPlekjeActivityScraper>();
        
        // Register Background Service (Scraper)
        services.AddHostedService<ScraperBackgroundService>();

        return services;
    }
}