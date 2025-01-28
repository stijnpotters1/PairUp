namespace PairUpApi.Configuration.Seeder;

public class SeedingService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SeedingService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var roleSeeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();
            await roleSeeder.SeedRolesAsync();

            var userSeeder = scope.ServiceProvider.GetRequiredService<AdminUserSeeder>();
            await userSeeder.SeedAdminUserAsync();
        }
    }
    
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}