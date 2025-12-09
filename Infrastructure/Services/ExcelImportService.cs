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
                        DocumentNumber = long.Parse(row["Documento"].ToString()),
                        FirstName = row["Nombres"].ToString(),
                        LastName = row["Apellidos"].ToString(),
                        DateOfBirth = DateTime.Parse(row["FechaNacimiento"].ToString()),
                        Address = row["Direccion"].ToString(),
                        PhoneNumber = long.Parse(row["Telefono"].ToString()),
                        Email = row["Email"].ToString(),
                        JobTitle = row["Cargo"].ToString(),
                        Salary = decimal.Parse(row["Salario"].ToString()),
                        HireDate = DateTime.Parse(row["FechaIngreso"].ToString()),
                        Status = row["Estado"].ToString(),
                        EducationLevel = row["NivelEducativo"].ToString(),
                        ProfessionalProfile = row["PerfilProfesional"].ToString(),
                        Department = row["Departamento"].ToString()
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
