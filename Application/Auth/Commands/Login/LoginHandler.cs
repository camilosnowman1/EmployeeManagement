using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Needed for FirstOrDefaultAsync

namespace Application.Auth.Commands.Login;

public class LoginHandler : IRequestHandler<LoginCommand, string>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IEmployeeRepository _employeeRepository;

    public LoginHandler(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtTokenGenerator jwtTokenGenerator, IEmployeeRepository employeeRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _employeeRepository = employeeRepository;
    }

    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Invalid credentials");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new Exception("Invalid credentials");
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        // Find associated employee
        var employees = await _employeeRepository.GetAllAsync();
        var employee = employees.FirstOrDefault(e => e.Email == request.Email);

        return _jwtTokenGenerator.GenerateToken(user, roles, employee);
    }
}
