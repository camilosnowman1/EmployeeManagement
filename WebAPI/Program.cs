using Application.Mappings;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using FluentValidation;
using Application.Employees.Commands.CreateEmployee;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Employee API", Version = "v1" });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// If connection string contains placeholders, try to replace them with environment variables
if (connectionString != null && connectionString.Contains("${DB_HOST}"))
{
    var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
    var db = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";
    var user = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
    var pass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
    var ssl = Environment.GetEnvironmentVariable("DB_SSLMODE") ?? "Prefer";

    connectionString = connectionString
        .Replace("${DB_HOST}", host)
        .Replace("${DB_PORT}", port)
        .Replace("${DB_NAME}", db)
        .Replace("${DB_USER}", user)
        .Replace("${DB_PASSWORD}", pass)
        .Replace("${DB_SSLMODE}", ssl);
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Dependency Injection
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Employees.Queries.GetEmployees.GetEmployeesQuery).Assembly));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// FluentValidation
builder.Services.AddValidatorsFromAssembly(typeof(CreateEmployeeValidator).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
// In production, we might want to keep Swagger enabled for this demo
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
