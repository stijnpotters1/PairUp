namespace PairUpApi.Configuration;

public static class ServiceContainer
{
    public static IServiceCollection InstantiateServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
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
        
        // Register Background Service (Scraper)
        services.AddHostedService<ScraperBackgroundService>();

        // Scoped custom services (Dependency injection for services and repositories)
        services.AddScoped<IService<Activity, ActivityResponse>, ActivityService>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IService<SubLevelCategory, SubLevelCategoryResponse>, SubLevelCategoryService>();
        services.AddScoped<ISubLevelCategoryRepository, SubLevelCategoryRepository>();

        services.AddSingleton<HttpClient>();
        
        // services.AddScoped<IWebScraper, TripAdvisorScraper>();
        services.AddScoped<IWebScraper, BijzonderPlekjeAccommodationScraper>();

        return services;
    }
}