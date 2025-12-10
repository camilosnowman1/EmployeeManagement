using Application.DTOs;
using MediatR;

namespace Application.Employees.Queries.GetEmployeeDetail;

public class GetEmployeeDetailQuery : IRequest<EmployeeDto>
{
    public string Email { get; set; } = string.Empty;
}
