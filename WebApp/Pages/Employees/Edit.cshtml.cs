using Application.DTOs;
using Application.Employees.Commands.UpdateEmployee;
using Application.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees;

[Authorize]
public class EditModel : PageModel
{
    private readonly IMediator _mediator;

    public EditModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public UpdateEmployeeCommand Employee { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));
        
        if (employee == null)
        {
            return NotFound();
        }

        Employee = new UpdateEmployeeCommand
        {
            Id = employee.Id,
            DocumentNumber = employee.DocumentNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            Address = employee.Address,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email,
            JobTitle = employee.JobTitle,
            Salary = employee.Salary,
            HireDate = employee.HireDate,
            Status = employee.Status,
            EducationLevel = employee.EducationLevel,
            ProfessionalProfile = employee.ProfessionalProfile,
            Department = employee.Department
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            // Ensure dates are UTC for PostgreSQL
            Employee.DateOfBirth = DateTime.SpecifyKind(Employee.DateOfBirth, DateTimeKind.Utc);
            Employee.HireDate = DateTime.SpecifyKind(Employee.HireDate, DateTimeKind.Utc);

            await _mediator.Send(Employee);
            TempData["Message"] = "Employee updated successfully.";
            return RedirectToPage("Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error updating employee: {ex.Message}";
            return Page();
        }
    }
}
