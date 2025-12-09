using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

[Authorize]
public class DashboardModel : PageModel
{
    private readonly IDashboardService _dashboardService;
    private readonly IGeminiService _geminiService;

    public DashboardModel(IDashboardService dashboardService, IGeminiService geminiService)
    {
        _dashboardService = dashboardService;
        _geminiService = geminiService;
    }

    public DashboardDto Stats { get; set; }
    
    [BindProperty]
    public string Query { get; set; }
    
    public string AiResponse { get; set; }

    public async Task OnGetAsync()
    {
        Stats = await _dashboardService.GetDashboardStatsAsync();
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        Stats = await _dashboardService.GetDashboardStatsAsync();
        
        if (!string.IsNullOrWhiteSpace(Query))
        {
            AiResponse = await _geminiService.QueryEmployeeDataAsync(Query);
        }
        
        return Page();
    }
}
