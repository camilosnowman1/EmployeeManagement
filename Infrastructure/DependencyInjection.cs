using Application.Interfaces;
using Infrastructure.Auth;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Configuration
        // We assume the connection string is handled by the presentation layer or we can read it here.
        // However, to keep it simple and consistent with existing code, we'll let the presentation layer handle DbContext configuration for now if it's complex,
        // OR we can move it here.
        
        // Let's move the services first, as that's the immediate fix for GeminiService.
        
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IPdfService, PdfService>();
        services.AddScoped<IExcelImportService, ExcelImportService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IDashboardService, DashboardService>();
        
        // Gemini Service
        services.AddHttpClient<IGeminiService, GeminiService>();
        
        // Auth
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
