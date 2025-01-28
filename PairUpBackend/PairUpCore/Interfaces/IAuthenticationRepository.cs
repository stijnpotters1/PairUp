namespace PairUpCore.Interfaces;

public interface IAuthenticationRepository
{
    Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest registerRequest);
    Task<AuthenticationResponse> LoginUserAsync(LoginRequest loginRequest);
}