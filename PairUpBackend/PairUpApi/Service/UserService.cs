namespace PairUpApi.Service;

public class UserService: IUserService
{
    private readonly IMapper _mapper;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserResponse ConvertToResponse(User user)
    {
        return _mapper.Map<UserResponse>(user);
    }

    public User ConvertToEntity(UserRequest userRequest)
    {
        return _mapper.Map<User>(userRequest);
    }

    public User EntityUpdate(User existingUser, User updatedUser)
    {
        _mapper.Map(updatedUser, existingUser);
        return existingUser;
    }
}