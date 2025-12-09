using MediatR;
using Application.DTOs;

namespace Application.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : IRequest<PaginatedResult<EmployeeDto>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public GetEmployeesQuery(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }
}
