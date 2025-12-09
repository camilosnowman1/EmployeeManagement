using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Application.DTOs;

namespace Application.Employees.Queries.GetEmployees;

public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, PaginatedResult<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeesHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var (employees, totalCount) = await _repository.GetPaginatedAsync(request.Page, request.PageSize);
        var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        
        return new PaginatedResult<EmployeeDto>(employeeDtos, totalCount, request.Page, request.PageSize);
    }
}
