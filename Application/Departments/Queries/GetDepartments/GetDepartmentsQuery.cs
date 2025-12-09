using MediatR;

namespace Application.Departments.Queries.GetDepartments;

public class GetDepartmentsQuery : IRequest<IEnumerable<string>>
{
}
