using Application.DTOs;
using Application.Employees.Commands.UpdateEmployee;
using Application.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees;

public class EditModel : PageModel
{
    private readonly IMediator _mediator;

    public EditModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public UpdateEmployeeCommand Employee { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var employeeDto = await _mediator.Send(new GetEmployeeByIdQuery(id));

        if (employeeDto == null)
        {
            return NotFound();
        }

        Employee = new UpdateEmployeeCommand
        {
            Id = employeeDto.Id,
            DocumentNumber = employeeDto.DocumentNumber,
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            DateOfBirth = employeeDto.DateOfBirth,
            Address = employeeDto.Address,
            PhoneNumber = employeeDto.PhoneNumber,
            Email = employeeDto.Email,
            JobTitle = employeeDto.JobTitle,
            Salary = employeeDto.Salary,
            HireDate = employeeDto.HireDate,
            Status = employeeDto.Status,
            EducationLevel = employeeDto.EducationLevel,
            ProfessionalProfile = employeeDto.ProfessionalProfile,
            Department = employeeDto.Department
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
            await _mediator.Send(Employee);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToPage("./Index");
    }
}
