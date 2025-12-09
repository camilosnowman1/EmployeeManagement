using Domain.Entities;

namespace Domain.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<(IEnumerable<Employee> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize);
    Task<Employee?> GetByEmailAsync(string email);
    Task<IEnumerable<string>> GetDistinctDepartmentsAsync();
    Task<Employee?> GetByIdAsync(Guid id);
    Task AddAsync(Employee employee);
    Task UpdateAsync(Employee employee);
    Task DeleteAsync(Guid id);
}
