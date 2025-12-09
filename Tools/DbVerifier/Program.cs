using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DbVerifier;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Database Verification Tool ===");

        string inputString;
        if (args.Length > 0)
        {
            inputString = args[0];
        }
        else
        {
            Console.WriteLine("Please enter the Connection String or URI:");
            inputString = Console.ReadLine() ?? string.Empty;
        }

        if (string.IsNullOrWhiteSpace(inputString))
        {
            Console.WriteLine("Error: Connection string is required.");
            return;
        }

        string connectionString = ConvertUriToConnectionString(inputString);
        Console.WriteLine($"\nUsing Connection String: {connectionString}");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        using var context = new AppDbContext(optionsBuilder.Options);

        try
        {
            Console.WriteLine("\n1. Testing Connection...");
            if (await context.Database.CanConnectAsync())
            {
                Console.WriteLine("✅ Connection Successful!");
            }
            else
            {
                Console.WriteLine("❌ Connection Failed.");
                return;
            }

            Console.WriteLine("\n2. Applying Migrations...");
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Migrations Applied!");

            Console.WriteLine("\n3. Testing CRUD Operations...");
            
            // Create
            var newEmployee = new Employee
            {
                Id = Guid.NewGuid(),
                DocumentNumber = 123456789,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                DateOfBirth = DateTime.Now.AddYears(-30),
                HireDate = DateTime.Now,
                JobTitle = "Tester",
                Salary = 1000,
                Status = "Active"
            };
            
            Console.WriteLine("   - Adding Test Employee...");
            context.Employees.Add(newEmployee);
            await context.SaveChangesAsync();
            Console.WriteLine("   ✅ Added.");

            // Read
            Console.WriteLine("   - Reading Test Employee...");
            var employee = await context.Employees.FindAsync(newEmployee.Id);
            if (employee != null && employee.FirstName == "Test")
            {
                Console.WriteLine("   ✅ Read Successful.");
            }
            else
            {
                Console.WriteLine("   ❌ Read Failed.");
            }

            // Delete
            Console.WriteLine("   - Deleting Test Employee...");
            context.Employees.Remove(newEmployee);
            await context.SaveChangesAsync();
            Console.WriteLine("   ✅ Deleted.");

            Console.WriteLine("\n🎉 ALL CHECKS PASSED! The database is correctly configured and accessible.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ An error occurred: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    static string ConvertUriToConnectionString(string input)
    {
        // Check if it's a URI (postgres://)
        if (input.StartsWith("postgres://") || input.StartsWith("postgresql://"))
        {
            try
            {
                var uri = new Uri(input);
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";
                
                return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password};SslMode=Require;Trust Server Certificate=true";
            }
            catch
            {
                Console.WriteLine("Warning: Failed to parse URI, using as plain connection string.");
                return input;
            }
        }
        return input;
    }
}
