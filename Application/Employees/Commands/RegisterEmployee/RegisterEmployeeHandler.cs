using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Application.DTOs;
using Application.Interfaces;

namespace Application.Employees.Commands.RegisterEmployee;

public class RegisterEmployeeHandler : IRequestHandler<RegisterEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;

    public RegisterEmployeeHandler(IEmployeeRepository repository, IMapper mapper, IEmailService emailService)
    {
        _repository = repository;
        _mapper = mapper;
        _emailService = emailService;
    }

    public async Task<EmployeeDto> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            DocumentNumber = request.DocumentNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = DateTime.SpecifyKind(request.DateOfBirth, DateTimeKind.Utc),
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            JobTitle = request.JobTitle,
            Salary = request.Salary,
            HireDate = DateTime.SpecifyKind(request.HireDate, DateTimeKind.Utc),
            Status = request.Status,
            EducationLevel = request.EducationLevel,
            ProfessionalProfile = request.ProfessionalProfile,
            Department = request.Department
        };

        await _repository.AddAsync(employee);

        // Send welcome email
        try
        {
            await _emailService.SendWelcomeEmailAsync(
                employee.Email,
                $"{employee.FirstName} {employee.LastName}"
            );
        }
        catch (Exception)
        {
            // Log error but don't fail registration
        }

        return _mapper.Map<EmployeeDto>(employee);
    }
}
