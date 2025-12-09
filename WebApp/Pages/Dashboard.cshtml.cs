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

    public DashboardModel(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public DashboardDto Stats { get; set; }

    public async Task OnGetAsync()
    {
        Stats = await _dashboardService.GetDashboardStatsAsync();
    }
}
