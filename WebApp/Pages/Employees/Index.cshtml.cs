using Application.DTOs;
using Application.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees;

public class IndexModel : PageModel
{
    private readonly IMediator _mediator;

    public IndexModel(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IEnumerable<EmployeeDto> Employees { get; set; } = new List<EmployeeDto>();

    public async Task OnGetAsync()
    {
        Employees = await _mediator.Send(new GetEmployeesQuery());
    }
}
