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
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 10;

    public async Task OnGetAsync(int page = 1, int pageSize = 10)
    {
        var result = await _mediator.Send(new GetEmployeesQuery(page, pageSize));
        Employees = result.Items;
        CurrentPage = result.Page;
        TotalPages = result.TotalPages;
        PageSize = result.PageSize;
    }
}
