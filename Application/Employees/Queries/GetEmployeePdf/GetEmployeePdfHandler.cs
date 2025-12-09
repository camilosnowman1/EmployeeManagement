using Application.Interfaces;
using Domain.Interfaces;
using MediatR;
using AutoMapper;
using Application.DTOs;

namespace Application.Employees.Queries.GetEmployeePdf;

public class GetEmployeePdfHandler : IRequestHandler<GetEmployeePdfQuery, byte[]>
{
    private readonly IEmployeeRepository _repository;
    private readonly IPdfService _pdfService;
    private readonly IMapper _mapper;

    public GetEmployeePdfHandler(IEmployeeRepository repository, IPdfService pdfService, IMapper mapper)
    {
        _repository = repository;
        _pdfService = pdfService;
        _mapper = mapper;
    }

    public async Task<byte[]> Handle(GetEmployeePdfQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(request.EmployeeId);
        if (employee == null)
        {
            return null; // Or throw NotFoundException
        }

        var employeeDto = _mapper.Map<EmployeeDto>(employee);
        return _pdfService.GenerateEmployeePdf(employeeDto);
    }
}
