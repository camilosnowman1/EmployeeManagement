using MediatR;
using Application.DTOs;

namespace Application.Employees.Commands.RegisterEmployee;

public class RegisterEmployeeCommand : IRequest<EmployeeDto>
{
    public long DocumentNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Address { get; set; }
    public long PhoneNumber { get; set; }
    public string Email { get; set; }
    public string JobTitle { get; set; }
    public decimal Salary { get; set; }
    public DateTime HireDate { get; set; }
    public string Status { get; set; }
    public string EducationLevel { get; set; }
    public string ProfessionalProfile { get; set; }
    public string Department { get; set; }
}
