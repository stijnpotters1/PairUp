namespace PairUpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _repository;
    private readonly IService<Role, RoleDto> _service;

    public RoleController(IRoleRepository repository, IService<Role, RoleDto> service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
    {
        IEnumerable<Role> roles = await _repository.GetRolesAsync();
        IEnumerable<RoleDto> roleResponse = roles.Select(fr => _service.ConvertToResponse(fr));
        return Ok(roleResponse);
    }
}