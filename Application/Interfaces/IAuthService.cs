using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<LoginResult> LoginEmployeeAsync(long documentNumber, string email);
}

public class LoginResult
{
    public string Token { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
}
