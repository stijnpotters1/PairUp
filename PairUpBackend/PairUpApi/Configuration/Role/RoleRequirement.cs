using Microsoft.AspNetCore.Authorization;

namespace PairUpApi.Configuration.Role;

public class RoleRequirement : IAuthorizationRequirement
{
    public string[] Roles { get; }

    public RoleRequirement(params string[] roles)
    {
        Roles = roles;
    }
}