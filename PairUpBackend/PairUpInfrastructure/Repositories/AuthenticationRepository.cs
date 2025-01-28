namespace PairUpInfrastructure.Repositories;

public class AuthenticationRepository : IAuthenticationRepository
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;

    public AuthenticationRepository(DataContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthenticationResponse> RegisterUserAsync(RegisterRequest registerRequest)
    {
        User? existingUser = await FindUserByEmail(registerRequest.Email!);
        if (existingUser != null)
        {
            throw new InvalidCredentialsException();
        }

        Role? userRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == "User");

        if (userRole == null)
        {
            throw new RoleNotFoundException();
        }

        if (!registerRequest.Password.Equals(registerRequest.ConfirmPassword))
        {
            throw new PasswordsDoNotMatchException();
        }
        
        if (registerRequest.Password.Length < 12)
        {
            throw new PasswordMustBeAtLeast12CharactersLongException();
        }
        
        var newUser = new User
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            RoleId = userRole.Id
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        User? storedUser = await FindUserByEmail(registerRequest.Email!);
        if (storedUser == null)
        {
            throw new InvalidCredentialsException();
        }

        string token = GenerateJwtToken(storedUser.Id);
        return new AuthenticationResponse(token);
    }


    public async Task<AuthenticationResponse> LoginUserAsync(LoginRequest loginRequest)
    {
        User? user = await FindUserByEmail(loginRequest.Email!);
        if (user == null)
        {
            throw new InvalidCredentialsException();
        }
        
        if (loginRequest.Password.Length < 12)
        {
            throw new PasswordMustBeAtLeast12CharactersLongException();
        }

        bool checkPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
        if (!checkPassword)
        {
            throw new InvalidCredentialsException();
        }

        return new AuthenticationResponse(GenerateJwtToken(user.Id));
    }

    private string GenerateJwtToken(Guid userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var userClaims = new[]
        {
            new Claim("userId", userId.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: credentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private async Task<User?> FindUserByEmail(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}