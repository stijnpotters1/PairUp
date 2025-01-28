namespace PairUpCore.Interfaces;

public interface IUserService
{
    UserResponse ConvertToResponse(User user);
    User ConvertToEntity(UserRequest userRequest);
    User EntityUpdate(User existingUser, User updatedUser);
}