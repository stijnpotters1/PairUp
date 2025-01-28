namespace PairUpCore.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> GetByIdAsync(Guid id, Guid? userIdClaim = null);
    Task<User> AddAsync(User user);
    Task<User> UpdateAsync(Guid id, User user, Guid? userIdClaim = null);
    Task<bool> DeleteAsync(Guid id, Guid? userIdClaim = null);
}