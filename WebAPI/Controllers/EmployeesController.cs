using Application.DTOs;
using Application.Employees.Commands.CreateEmployee;
using Application.Employees.Commands.DeleteEmployee;
using Application.Employees.Commands.UpdateEmployee;
using Application.Employees.Commands.RegisterEmployee;
using Application.Interfaces;
using Application.Employees.Queries.GetEmployeeById;
using Application.Employees.Queries.GetEmployees;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;

    public EmployeesController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResult<EmployeeDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var query = new GetEmployeesQuery(page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> DownloadPdf(Guid id)
    {
        var query = new Application.Employees.Queries.GetEmployeePdf.GetEmployeePdfQuery(id);
        var pdfBytes = await _mediator.Send(query);

        if (pdfBytes == null)
            return NotFound();

        return File(pdfBytes, "application/pdf", $"Employee_{id}.pdf");
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<EmployeeDto>> GetMe()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                    ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var query = new Application.Employees.Queries.GetMyInfo.GetMyInfoQuery(email);
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound("Employee profile not found for this user.");

        return Ok(result);
    }

    [HttpGet("me/pdf")]
    [Authorize]
    public async Task<IActionResult> DownloadMyPdf()
    {
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value 
                    ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(email))
            return Unauthorized();

        var infoQuery = new Application.Employees.Queries.GetMyInfo.GetMyInfoQuery(email);
        var employeeDto = await _mediator.Send(infoQuery);

        if (employeeDto == null)
            return NotFound("Employee profile not found.");

        var pdfQuery = new Application.Employees.Queries.GetEmployeePdf.GetEmployeePdfQuery(employeeDto.Id);
        var pdfBytes = await _mediator.Send(pdfQuery);

        return File(pdfBytes, "application/pdf", $"My_CV.pdf");
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetById(Guid id)
    {
        var query = new GetEmployeeByIdQuery(id);
        var result = await _mediator.Send(query);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<EmployeeDto>> Register(RegisterEmployeeCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> EmployeeLogin([FromBody] EmployeeLoginRequest request)
    {
        try
        {
            var result = await _authService.LoginEmployeeAsync(request.DocumentNumber, request.Email);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> Create(CreateEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateEmployeeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        try
        {
            await _mediator.Send(command);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEmployeeCommand(id));
        return NoContent();
    }
}
