using Application.DTOs;
using MediatR;

namespace Application.Employees.Queries.GetMyInfo;

public class GetMyInfoQuery : IRequest<EmployeeDto>
{
    public string Email { get; set; }

    public GetMyInfoQuery(string email)
    {
        Email = email;
    }
}
