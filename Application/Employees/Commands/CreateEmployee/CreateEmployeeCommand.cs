using MediatR;
using Application.DTOs;

namespace Application.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommand : IRequest<EmployeeDto>
{
    public long DocumentNumber { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; } = string.Empty;
    public long PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string EducationLevel { get; set; } = string.Empty;
    public string ProfessionalProfile { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}
