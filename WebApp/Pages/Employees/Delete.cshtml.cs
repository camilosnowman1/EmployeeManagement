using Application.DTOs;
using Application.Employees.Commands.DeleteEmployee;
using Application.Employees.Queries.GetEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees;

public class DeleteModel : PageModel
{
    private readonly IMediator _mediator;

    public DeleteModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    [BindProperty]
    public EmployeeDto Employee { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(Guid id)
    {
        var employee = await _mediator.Send(new GetEmployeeByIdQuery(id));

        if (employee == null)
        {
            return NotFound();
        }

        Employee = employee;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return RedirectToPage("./Index");
    }
}
