using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using AutoMapper;

namespace Application.Employees.Commands.ImportEmployees;

public class ImportEmployeesHandler : IRequestHandler<ImportEmployeesCommand, int>
{
    private readonly IEmployeeRepository _repository;
    private readonly IExcelImportService _excelImportService;
    private readonly IMapper _mapper;

    public ImportEmployeesHandler(IEmployeeRepository repository, IExcelImportService excelImportService, IMapper mapper)
    {
        _repository = repository;
        _excelImportService = excelImportService;
        _mapper = mapper;
    }

    public async Task<int> Handle(ImportEmployeesCommand request, CancellationToken cancellationToken)
    {
        var employeeDtos = _excelImportService.ImportEmployees(request.FileStream);
        int count = 0;

        foreach (var dto in employeeDtos)
        {
            // Check if employee already exists (e.g., by DocumentNumber)
            // For simplicity, we'll assume we just add them or update if we had an Update logic based on ID.
            // Since DTO doesn't have ID for new ones, we map to Entity.
            
            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                DocumentNumber = dto.DocumentNumber,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateOfBirth = DateTime.SpecifyKind(dto.DateOfBirth, DateTimeKind.Utc),
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                JobTitle = dto.JobTitle,
                Salary = dto.Salary,
                HireDate = DateTime.SpecifyKind(dto.HireDate, DateTimeKind.Utc),
                Status = dto.Status,
                EducationLevel = dto.EducationLevel,
                ProfessionalProfile = dto.ProfessionalProfile,
                Department = dto.Department
            };
            
            // Basic validation or check could go here
            
            await _repository.AddAsync(employee);
            count++;
        }

        return count;
    }
}
