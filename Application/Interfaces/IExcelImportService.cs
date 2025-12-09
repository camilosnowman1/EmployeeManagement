using Application.DTOs;

namespace Application.Interfaces;

public interface IExcelImportService
{
    IEnumerable<EmployeeDto> ImportEmployees(Stream fileStream);
}
