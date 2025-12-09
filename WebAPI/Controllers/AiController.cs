using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AiController : ControllerBase
{
    private readonly IGeminiService _geminiService;

    public AiController(IGeminiService geminiService)
    {
        _geminiService = geminiService;
    }

    [HttpPost("query")]
    public async Task<IActionResult> Query([FromBody] AiQueryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Query))
        {
            return BadRequest("Query cannot be empty.");
        }

        var response = await _geminiService.QueryEmployeeDataAsync(request.Query);
        return Ok(new { answer = response });
    }
}

public record AiQueryRequest(string Query);
