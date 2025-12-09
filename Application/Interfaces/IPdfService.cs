using Application.DTOs;

namespace Application.Interfaces;

public interface IPdfService
{
    byte[] GenerateEmployeePdf(EmployeeDto employee);
}
