namespace PairUpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;
    private readonly IUserService _service;

    public UserController(IUserRepository repository, IUserService service)
    {
        _repository = repository;
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsersAsync()
    {
        IEnumerable<User> users = await _repository.GetAllAsync();
        IEnumerable<UserResponse> userResponse = users.Select(fr => _service.ConvertToResponse(fr));
        return Ok(userResponse);
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<ActionResult<UserResponse>> GetUserAsync(Guid id)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        User user = await _repository.GetByIdAsync(id, Guid.Parse(userIdClaim));
        return Ok(_service.ConvertToResponse(user));
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<UserResponse>> PostUserAsync([FromBody] UserRequest userRequest)
    {
        User user = _service.ConvertToEntity(userRequest);
        await _repository.AddAsync(user);
        return Ok(_service.ConvertToResponse(user));
    }
    
    [HttpPut("{id}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<ActionResult<UserResponse>> UpdateUserAsync(Guid id, [FromBody] UserRequest userRequest)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        User user = await _repository.UpdateAsync(id, _service.ConvertToEntity(userRequest), Guid.Parse(userIdClaim));
        return Ok(_service.ConvertToResponse(user));
    }
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<ActionResult<bool>> DeleteUser(Guid id)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

        Guid userIdGuid = Guid.Parse(userIdClaim);

        var response = await _repository.DeleteAsync(id, userIdGuid);

        return Ok(response);
    }
}