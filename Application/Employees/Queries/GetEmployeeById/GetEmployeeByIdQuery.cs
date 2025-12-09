using MediatR;
using Application.DTOs;

namespace Application.Employees.Queries.GetEmployeeById;

public class GetEmployeeByIdQuery : IRequest<EmployeeDto?>
{
    public Guid Id { get; set; }

    public GetEmployeeByIdQuery(Guid id)
    {
        Id = id;
    }
}
