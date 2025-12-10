using Application.Mappings;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Application.Employees.Commands.CreateEmployee;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddRouting(options => options.LowercaseUrls = true);


// Database Configuration
var connectionString = ConnectionStringHelper.GetConnectionString(builder.Configuration);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Identity Configuration
builder.Services.AddDefaultIdentity<Microsoft.AspNetCore.Identity.IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

// Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IPdfService, PdfService>();
builder.Services.AddScoped<IExcelImportService, ExcelImportService>();
builder.Services.AddScoped<IEmailService, EmailService>(); // <-- Corrected typo here
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddHttpClient<IGeminiService, GeminiService>();
builder.Services.AddScoped<IJwtTokenGenerator, Infrastructure.Auth.JwtTokenGenerator>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Employees.Queries.GetEmployees.GetEmployeesQuery).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateEmployeeValidator).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// --- Use URL Rewriting to redirect the root URL to /dashboard ---
var rewriteOptions = new RewriteOptions().AddRedirect("^$", "dashboard", 302);
app.UseRewriter(rewriteOptions);
// --- End of Code ---

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
