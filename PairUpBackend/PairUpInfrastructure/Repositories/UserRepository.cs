namespace PairUpInfrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IUserService _service;

    public UserRepository(DataContext context, IUserService service)
    {
        _context = context;
        _service = service;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Include(u => u.Role).
            ToListAsync();
    }

    public async Task<User> GetByIdAsync(Guid id, Guid? userIdClaim)
    {
        User? requestingUser = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == userIdClaim);

        if (requestingUser == null)
        {
            throw new UserNotFoundException();
        }

        bool isAdmin = requestingUser.Role.Name == "Admin";

        if (!isAdmin && requestingUser.Id != id)
        {
            throw new UnauthorizedToMakeThisRequestException();
        }

        User? user = await _context.Users
            .Include(u => u.Role)
            .Include(u => u.LikedActivities)
                .ThenInclude(r => r.ActivitySubLevelCategories)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return user;
    }

    public async Task<User> AddAsync(User user)
    {
        var role = await _context.Roles.FindAsync(user.RoleId);

        if (role == null)
        {
            throw new RoleNotFoundException();
        }

        user.Role = role;

        if (user.Password.Length < 12)
        {
            throw new PasswordMustBeAtLeast12CharactersLongException();
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        
        bool exists = await _context.Users.AnyAsync(fr => fr.Email == user.Email);
        if (exists)
        {
            throw new UserAlreadyExistsException();
        }

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(Guid id, User updatedUser, Guid? userIdClaim)
    {
        User? requestingUser = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userIdClaim);
        
        if (requestingUser == null)
        {
            throw new UserNotFoundException();
        }

        bool isAdmin = requestingUser.Role.Name == "Admin";

        User? existingUser = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
        if (existingUser == null)
        {
            throw new UserNotFoundException();
        }

        if (!isAdmin && requestingUser.Id != id)
        {
            throw new UnauthorizedToMakeThisRequestException();
        }

        if (!isAdmin)
        {
            updatedUser.RoleId = existingUser.RoleId;
            updatedUser.Role = existingUser.Role;
        }
        else
        {
            var role = await _context.Roles.FindAsync(updatedUser.RoleId);
            if (role == null)
            {
                throw new RoleNotFoundException();
            }

            existingUser.RoleId = updatedUser.RoleId;
            existingUser.Role = role;
        }

        if (!string.IsNullOrEmpty(updatedUser.Password) && !BCrypt.Net.BCrypt.Verify(updatedUser.Password, existingUser.Password))
        {
            if (updatedUser.Password.Length < 12)
            {
                throw new PasswordMustBeAtLeast12CharactersLongException();
            }
            
            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
        }

        _service.EntityUpdate(existingUser, updatedUser);

        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid? userIdClaim)
    {
        User? requestingUser = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userIdClaim);
        
        if (requestingUser == null)
        {
            throw new UserNotFoundException();
        }

        bool isAdmin = requestingUser.Role.Name == "Admin";

        User? targetUser = await _context.Users.FindAsync(id);
        if (targetUser == null)
        {
            throw new UserNotFoundException();
        }

        if (!isAdmin && !requestingUser.Id.Equals(targetUser.Id))
        {
            throw new UnauthorizedToMakeThisRequestException();
        }

        _context.Users.Remove(targetUser);
        await _context.SaveChangesAsync();
        return true;
    }
}