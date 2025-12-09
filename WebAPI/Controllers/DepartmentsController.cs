using Application.Departments.Queries.GetDepartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepartmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetDepartments()
    {
        var query = new GetDepartmentsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
