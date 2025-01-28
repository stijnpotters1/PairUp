namespace PairUpInfrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DataContext _context;

    public RoleRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Role>> GetRolesAsync()
    {
        return await _context.Roles
            .ToListAsync();
    }
}