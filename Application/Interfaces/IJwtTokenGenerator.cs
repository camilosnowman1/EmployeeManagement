using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUser user, IList<string> roles, Employee? employee = null);
}
