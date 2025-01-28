namespace PairUpApi.Configuration.Services;

public static class AppSettingsConfiguration
{
    public static IServiceCollection ConfigureAppSettings(this IServiceCollection services, WebApplicationBuilder builder)
    {
        Env.Load();

        builder.Configuration["ConnectionStrings:DatabaseConnection"] = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        builder.Configuration["Jwt:SecretKey"] = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");
        builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        builder.Configuration["Seeder:Admin:FirstName"] = Environment.GetEnvironmentVariable("SEEDER_ADMIN_FIRSTNAME");
        builder.Configuration["Seeder:Admin:LastName"] = Environment.GetEnvironmentVariable("SEEDER_ADMIN_LASTNAME");
        builder.Configuration["Seeder:Admin:Email"] = Environment.GetEnvironmentVariable("SEEDER_ADMIN_EMAIL");
        builder.Configuration["Seeder:Admin:Password"] = Environment.GetEnvironmentVariable("SEEDER_ADMIN_PASSWORD");
        
        var roles = Environment.GetEnvironmentVariable("SEEDER_ROLES")?.Split(',', StringSplitOptions.RemoveEmptyEntries);
        if (roles != null)
        {
            builder.Configuration["Seeder:Role"] = string.Join(",", roles);
        }
        
        return services;
    }
}