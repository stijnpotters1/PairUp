namespace PairUpCore.Interfaces;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetRolesAsync();
}