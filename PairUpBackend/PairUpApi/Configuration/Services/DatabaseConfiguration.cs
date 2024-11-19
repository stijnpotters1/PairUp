namespace PairUpApi.Configuration.Services;

public static class DatabaseConfiguration
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, WebApplicationBuilder builder)
    {
        Env.Load();
        
        var dbConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");

        if (!string.IsNullOrEmpty(dbConnectionString))
        {
            builder.Configuration["ConnectionStrings:DatabaseConnection"] = dbConnectionString;
        }
        
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

        return services;
    }
}