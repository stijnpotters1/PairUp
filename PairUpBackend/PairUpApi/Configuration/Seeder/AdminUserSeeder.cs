namespace PairUpApi.Configuration.Seeder;

public class AdminUserSeeder
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AdminUserSeeder(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task SeedAdminUserAsync()
    {
        var adminSection = _configuration.GetSection("Seeder:Admin");
        var adminFirstName = adminSection["FirstName"];
        var adminLastName = adminSection["LastName"];
        var adminEmail = adminSection["Email"];
        var adminPassword = adminSection["Password"];

        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

        if (!await _context.Users.AnyAsync(u => u.Email == adminEmail))
        {
            var adminUser = new User
            {
                FirstName = adminFirstName,
                LastName = adminLastName,
                Email = adminEmail!,
                Password = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                RoleId = adminRole.Id,
                Role = adminRole
            };

            _context.Users.Add(adminUser);
            await _context.SaveChangesAsync();
        }
    }
}