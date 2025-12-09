using Domain.Interfaces;
using MediatR;

namespace Application.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand>
{
    private readonly IEmployeeRepository _repository;

    public UpdateEmployeeHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.Id);
        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with Id {request.Id} not found.");
        }

        employee.DocumentNumber = request.DocumentNumber;
        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.DateOfBirth = request.DateOfBirth;
        employee.Address = request.Address;
        employee.PhoneNumber = request.PhoneNumber;
        employee.Email = request.Email;
        employee.JobTitle = request.JobTitle;
        employee.Salary = request.Salary;
        employee.HireDate = request.HireDate;
        employee.Status = request.Status;
        employee.EducationLevel = request.EducationLevel;
        employee.ProfessionalProfile = request.ProfessionalProfile;
        employee.Department = request.Department;

        await _repository.UpdateAsync(employee);
    }
}
