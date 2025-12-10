using Application.DTOs;
using Application.Employees.Commands.CreateEmployee;
using Application.Employees.Commands.DeleteEmployee;
using Application.Employees.Commands.ImportEmployees;
using Application.Employees.Queries.GetEmployees;
using Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages.Employees;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IMediator _mediator;
    private readonly IPdfService _pdfService;

    public IndexModel(IMediator mediator, IPdfService pdfService)
    {
        _mediator = mediator;
        _pdfService = pdfService;
    }

    public PaginatedResult<EmployeeDto> Employees { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    
    public int PageSize { get; set; } = 10;
    
    public string? Message { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        var query = new GetEmployeesQuery(CurrentPage, PageSize);
        Employees = await _mediator.Send(query);
        
        // Preserve messages from TempData
        Message = TempData["Message"]?.ToString();
        ErrorMessage = TempData["ErrorMessage"]?.ToString();
    }

    public async Task<IActionResult> OnPostUploadExcelAsync(IFormFile excelFile)
    {
        if (excelFile == null || excelFile.Length == 0)
        {
            TempData["ErrorMessage"] = "Please select a valid Excel file.";
            return RedirectToPage();
        }

        if (!excelFile.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) &&
            !excelFile.FileName.EndsWith(".xls", StringComparison.OrdinalIgnoreCase))
        {
            TempData["ErrorMessage"] = "Only Excel files (.xlsx, .xls) are allowed.";
            return RedirectToPage();
        }

        try
        {
            using var stream = excelFile.OpenReadStream();
            var command = new ImportEmployeesCommand(stream);
            var importedCount = await _mediator.Send(command);
            
            TempData["Message"] = $"Successfully imported {importedCount} employees from Excel.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error importing Excel: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        try
        {
            await _mediator.Send(new DeleteEmployeeCommand(id));
            TempData["Message"] = "Employee deleted successfully.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error deleting employee: {ex.Message}";
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetDownloadPdfAsync(Guid id)
    {
        try
        {
            var query = new Application.Employees.Queries.GetEmployeePdf.GetEmployeePdfQuery(id);
            var pdfBytes = await _mediator.Send(query);

            if (pdfBytes == null)
                return NotFound();

            return File(pdfBytes, "application/pdf", $"Employee_{id}.pdf");
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Error generating PDF.";
            return RedirectToPage();
        }
    }
}
