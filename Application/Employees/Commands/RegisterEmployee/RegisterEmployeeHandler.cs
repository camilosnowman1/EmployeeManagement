using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Employees.Commands.RegisterEmployee;

public class RegisterEmployeeHandler : IRequestHandler<RegisterEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<RegisterEmployeeHandler> _logger;

    public RegisterEmployeeHandler(
        IEmployeeRepository employeeRepository, 
        UserManager<IdentityUser> userManager, 
        IMapper mapper,
        IEmailService emailService,
        ILogger<RegisterEmployeeHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _userManager = userManager;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<EmployeeDto> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        // 1. Create Identity User
        var user = new IdentityUser { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"User registration failed: {errors}");
        }

        // 2. Create Employee
        var employee = _mapper.Map<Employee>(request);
        // Ensure Id is generated if not handled by DB (usually DB handles it or we generate Guid)
        if (employee.Id == Guid.Empty) employee.Id = Guid.NewGuid();
        
        await _employeeRepository.AddAsync(employee);

        // 3. Send Welcome Email
        _logger.LogInformation("[RegisterEmployeeHandler] Attempting to send welcome email to {Email}", request.Email);
        try
        {
            await _emailService.SendWelcomeEmailAsync(request.Email, $"{request.FirstName} {request.LastName}");
            _logger.LogInformation("[RegisterEmployeeHandler] Email sent successfully to {Email}", request.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[RegisterEmployeeHandler] WARNING: Failed to send welcome email");
        }

        return _mapper.Map<EmployeeDto>(employee);
    }
}
