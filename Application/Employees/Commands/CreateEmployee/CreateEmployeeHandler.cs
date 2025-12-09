using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Application.DTOs;

namespace Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeHandler : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public CreateEmployeeHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
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
        return _mapper.Map<EmployeeDto>(employee);
    }
}
