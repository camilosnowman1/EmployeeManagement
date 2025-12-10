using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IEmployeeRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(IEmployeeRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<LoginResult> LoginEmployeeAsync(long documentNumber, string email)
    {
        var employee = await _repository.GetByEmailAsync(email);
        
        if (employee == null || employee.DocumentNumber != documentNumber)
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Generate JWT token
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "YourSuperSecretKeyForJWTTokenGeneration12345");
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new Claim("DocumentNumber", employee.DocumentNumber.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new LoginResult
        {
            Token = tokenString,
            Email = employee.Email,
            FullName = $"{employee.FirstName} {employee.LastName}"
        };
    }
}
