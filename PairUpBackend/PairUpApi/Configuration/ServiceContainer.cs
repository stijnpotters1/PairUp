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
        
        // Add swagger configuration + authentication 
        services.ConfigureSwagger();

        // Configure HTTPS redirection
        services.AddHttpsRedirection(options => { options.HttpsPort = 443; });

        // Database Configuration
        services.ConfigureDatabase(builder);
        
        // Automapper Configuration
        services.ConfigureMapping();
        
        // Authentication Configuration
        services.ConfigureAuthentication(builder.Configuration);

        // Role seeder Configuration
        services.AddScoped<RoleSeeder>();
        services.AddScoped<AdminUserSeeder>();

        // Register the role seeding service
        services.AddHostedService<SeedingService>();

        // Scoped custom services (Dependency injection for services and repositories)
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IService<Activity, ActivityResponse>, ActivityService>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IService<SubLevelCategory, SubLevelCategoryResponse>, SubLevelCategoryService>();
        services.AddScoped<ISubLevelCategoryRepository, SubLevelCategoryRepository>();
        services.AddScoped<IService<PairUpCore.Models.Role, RoleDto>, RoleService>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        
        // Scraper registration
        services.AddScoped<BaseWebScraper, BijzonderPlekjeAccommodationScraper>();
        services.AddScoped<BaseWebScraper, BijzonderPlekjeActivityScraper>();
        
        // Register Background Service (Scraper)
        services.AddHostedService<ScraperBackgroundService>();

        return services;
    }
}