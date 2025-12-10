using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

// Note: The 'using Application.DTOs;' is removed in favor of fully qualifying the type below to bypass a Docker cache issue.

namespace WebApp.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IDashboardService _dashboardService;
        private readonly IGeminiService _geminiService;

        public IndexModel(IDashboardService dashboardService, IGeminiService geminiService)
        {
            _dashboardService = dashboardService;
            _geminiService = geminiService;
        }

        // The type is now fully qualified to avoid compilation errors due to Docker's aggressive caching.
        public Application.DTOs.DashboardDto DashboardStats { get; set; }

        [BindProperty]
        public string AiQuery { get; set; }
        public string AiResponse { get; set; }

        public async Task OnGetAsync()
        {
            DashboardStats = await _dashboardService.GetDashboardStatsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(AiQuery))
            {
                AiResponse = "Please enter a question.";
                return Page();
            }

            try
            {
                AiResponse = await _geminiService.QueryEmployeeDataAsync(AiQuery);
            }
            catch (System.Exception ex)
            {
                AiResponse = $"Error calling AI service: {ex.Message}";
            }

            // We need to reload the stats as they are not persisted across requests
            DashboardStats = await _dashboardService.GetDashboardStatsAsync();
            
            return Page();
        }
    }
}
