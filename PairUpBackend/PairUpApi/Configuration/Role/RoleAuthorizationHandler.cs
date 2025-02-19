namespace PairUpApi.Configuration.Role;

public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
{
    private readonly DataContext _context;

    public RoleAuthorizationHandler(DataContext context)
    {
        _context = context;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        if (userIdClaim == null)
        {
            context.Fail();
            return;
        }

        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userIdClaim));

        if (user == null)
        {
            context.Fail();
            return;
        }

        if (requirement.Roles.Contains(user.Role.Name.Trim(), StringComparer.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}