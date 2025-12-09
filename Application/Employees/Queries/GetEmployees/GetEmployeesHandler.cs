using AutoMapper;
using Domain.Interfaces;
using MediatR;
using Application.DTOs;

namespace Application.Employees.Queries.GetEmployees;

public class GetEmployeesHandler : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetEmployeesHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<EmployeeDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}
