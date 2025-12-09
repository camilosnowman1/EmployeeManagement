namespace Application.DTOs;

public class DashboardDto
{
    public int TotalEmployees { get; set; }
    public int EmployeesOnVacation { get; set; }
    public int ActiveEmployees { get; set; }
    public Dictionary<string, int> EmployeesByDepartment { get; set; } = new();
}
