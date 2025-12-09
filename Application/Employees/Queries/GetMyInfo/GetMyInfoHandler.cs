using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace Application.Employees.Queries.GetMyInfo;

public class GetMyInfoHandler : IRequestHandler<GetMyInfoQuery, EmployeeDto>
{
    private readonly IEmployeeRepository _repository;
    private readonly IMapper _mapper;

    public GetMyInfoHandler(IEmployeeRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<EmployeeDto> Handle(GetMyInfoQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByEmailAsync(request.Email);
        if (employee == null)
        {
            return null;
        }

        return _mapper.Map<EmployeeDto>(employee);
    }
}
