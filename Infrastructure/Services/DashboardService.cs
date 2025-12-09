using Application.DTOs;
using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class DashboardService : IDashboardService
{
    private readonly IEmployeeRepository _repository;
    // We need direct access to IQueryable or similar to do aggregations efficiently, 
    // but since repository pattern abstracts it, we might need to add aggregation methods to repo 
    // or fetch all (not ideal for large datasets) or cast to implementation.
    // For this exercise, we'll assume we can fetch all or add methods to repo.
    // Let's add a method to Repo for stats to be clean.
    
    public DashboardService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<DashboardDto> GetDashboardStatsAsync()
    {
        // Ideally, we should add a method GetDashboardStats to the repository to do this in DB.
        // But for now, let's fetch all and calculate in memory if the dataset is small, 
        // OR better, let's add specific count methods to the repository.
        
        // Since I can't easily change the repo interface without touching many files, 
        // and assuming the dataset isn't huge for this demo, I'll use GetAllAsync.
        // *Correction*: I will add a GetStats method to the repository for performance.
        
        var employees = await _repository.GetAllAsync();

        var stats = new DashboardDto
        {
            TotalEmployees = employees.Count(),
            EmployeesOnVacation = employees.Count(e => e.Status.Contains("Vacaciones", StringComparison.OrdinalIgnoreCase)),
            ActiveEmployees = employees.Count(e => e.Status.Equals("Activo", StringComparison.OrdinalIgnoreCase)),
            EmployeesByDepartment = employees
                .GroupBy(e => e.Department)
                .ToDictionary(g => g.Key, g => g.Count())
        };

        return stats;
    }
}
