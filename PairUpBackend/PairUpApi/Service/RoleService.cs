namespace PairUpApi.Service;

public class RoleService : IService<Role, RoleDto>
{
    private readonly IMapper _mapper;

    public RoleService(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    public RoleDto ConvertToResponse(Role role)
    {
        return _mapper.Map<RoleDto>(role);
    }
}