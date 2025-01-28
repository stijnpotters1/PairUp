namespace PairUpApi.Configuration.Seeder;

public class RoleSeeder
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public RoleSeeder(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    public async Task SeedRolesAsync()
    {
        var roles = _configuration["Seeder:Role"]?.Split(',', StringSplitOptions.RemoveEmptyEntries);

        if (roles == null || roles.Length == 0)
        {
            Console.WriteLine("No roles found in configuration to seed.");
            return;
        }  
        
        foreach (var roleName in roles)
        {
            if (!await _context.Roles.AnyAsync(r => r.Name == roleName))
            {
                _context.Roles.Add(new PairUpCore.Models.Role { Name = roleName });
            }
        }

        await _context.SaveChangesAsync();
    }
}