using Application.DTOs;
using Application.Interfaces;
using ExcelDataReader;
using System.Data;

namespace Infrastructure.Services;

public class ExcelImportService : IExcelImportService
{
    public ExcelImportService()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
    }

    public IEnumerable<EmployeeDto> ImportEmployees(Stream fileStream)
    {
        var employees = new List<EmployeeDto>();

        using (var reader = ExcelReaderFactory.CreateReader(fileStream))
        {
            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            var dataTable = result.Tables[0];

            foreach (DataRow row in dataTable.Rows)
            {
                try
                {
                    var employee = new EmployeeDto
                    {
                        DocumentNumber = long.Parse(row[0]?.ToString() ?? "0"),
                        FirstName = row[1]?.ToString() ?? string.Empty,
                        LastName = row[2]?.ToString() ?? string.Empty,
                        DateOfBirth = DateTime.SpecifyKind(DateTime.Parse(row[3]?.ToString() ?? DateTime.Now.ToString()), DateTimeKind.Utc),
                        Address = row[4]?.ToString() ?? string.Empty,
                        PhoneNumber = long.Parse(row[5]?.ToString() ?? "0"),
                        Email = row[6]?.ToString() ?? string.Empty,
                        JobTitle = row[7]?.ToString() ?? string.Empty,
                        Salary = decimal.Parse(row[8]?.ToString() ?? "0"),
                        HireDate = DateTime.SpecifyKind(DateTime.Parse(row[9]?.ToString() ?? DateTime.Now.ToString()), DateTimeKind.Utc),
                        Status = row[10]?.ToString() ?? string.Empty,
                        EducationLevel = row[11]?.ToString() ?? string.Empty,
                        ProfessionalProfile = row[12]?.ToString() ?? string.Empty,
                        Department = row[13]?.ToString() ?? string.Empty
                    };
                    employees.Add(employee);
                }
                catch (Exception ex)
                {
                    // Log error or skip row
                    Console.WriteLine($"Error parsing row: {ex.Message}");
                }
            }
        }

        return employees;
    }
}
