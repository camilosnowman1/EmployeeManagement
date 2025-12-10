using Application.DTOs;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace Application.Employees.Queries.GetEmployeeDetail;

public class GetEmployeeDetailHandler : IRequestHandler<GetEmployeeDetailQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;

    public GetEmployeeDetailHandler(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
    {
        var employees = await _employeeRepository.GetAllAsync();
        var employee = employees.FirstOrDefault(e => e.Email == request.Email);

        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        return _mapper.Map<EmployeeDto>(employee);
    }
}
