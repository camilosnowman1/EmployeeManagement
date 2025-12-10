using Application.DTOs;
using Application.Employees.Commands.CreateEmployee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IMediator _mediator;

    public CreateModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public CreateEmployeeCommand Employee { get; set; } = new()
    {
        DateOfBirth = DateTime.Today.AddYears(-25),
        HireDate = DateTime.Today,
        Status = "Activo"
    };

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
        // Initialize defaults
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
            TempData["Message"] = "Employee created successfully.";
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error creating employee: {ex.Message}";
            return Page();
        }
    }
}
