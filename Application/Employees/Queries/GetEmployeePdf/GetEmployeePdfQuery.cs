using MediatR;

namespace Application.Employees.Queries.GetEmployeePdf;

public class GetEmployeePdfQuery : IRequest<byte[]>
{
    public Guid EmployeeId { get; set; }

    public GetEmployeePdfQuery(Guid employeeId)
    {
        EmployeeId = employeeId;
    }
}
