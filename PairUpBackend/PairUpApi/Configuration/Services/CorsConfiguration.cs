namespace PairUpApi.Configuration.Services;

public static class CorsConfiguration
{
    private const string MyAllowSpecificOrigins = "AllowSpecificOrigins";
    
    //todo change the allowed origins when going to production
    private static readonly string[] AllowedOrigins = 
    {
        "http://localhost:5173"
    };

    public static IServiceCollection ConfigureCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(MyAllowSpecificOrigins,
                policy =>
                {
                    policy.WithOrigins(AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        return services;
    }
}