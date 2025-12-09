using Domain.Interfaces;
using MediatR;

namespace Application.Departments.Queries.GetDepartments;

public class GetDepartmentsHandler : IRequestHandler<GetDepartmentsQuery, IEnumerable<string>>
{
    private readonly IEmployeeRepository _repository;

    public GetDepartmentsHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<string>> Handle(GetDepartmentsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetDistinctDepartmentsAsync();
    }
}
