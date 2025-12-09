using MediatR;
using Application.DTOs;

namespace Application.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
{
}
