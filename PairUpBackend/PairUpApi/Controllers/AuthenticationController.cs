namespace PairUpApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationRepository _repository;

    public AuthenticationController(IAuthenticationRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResponse>> Register (RegisterRequest registerUserDto)
    {
        var result = await _repository.RegisterUserAsync(registerUserDto);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Login (LoginRequest loginRequest)
    {
        var result = await _repository.LoginUserAsync(loginRequest);
        return Ok(result);
    }
}