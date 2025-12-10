using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Interfaces; // For IEmployeeRepository to fetch context data

namespace Infrastructure.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IEmployeeRepository _employeeRepository;

    public GeminiService(HttpClient httpClient, IConfiguration configuration, IEmployeeRepository employeeRepository)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GEMINI_API_KEY"];
        _employeeRepository = employeeRepository;
    }

    public async Task<string> QueryEmployeeDataAsync(string naturalLanguageQuery)
    {
        if (string.IsNullOrEmpty(_apiKey))
        {
            return "Error: Gemini API Key is missing.";
        }

        // 1. Fetch relevant data context (simplified for this demo: fetch all employees summary)
        // In a real app, we would use RAG or function calling.
        var employees = await _employeeRepository.GetAllAsync();
        var employeeSummary = string.Join("\n", employees.Select(e => 
            $"- {e.FirstName} {e.LastName}, Cargo: {e.JobTitle}, Dept: {e.Department}, Estado: {e.Status}, Salario: {e.Salary}"));

        var prompt = $@"
Act as an HR assistant. Answer the following question based ONLY on the employee data provided below.
If the answer cannot be found in the data, say 'I don't have that information'.

Employee Data:
{employeeSummary}

Question: {naturalLanguageQuery}
Answer:";

        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            return $"Error calling Gemini API: {response.StatusCode}";
        }

        var responseString = await response.Content.ReadAsStringAsync();
        
        try 
        {
            using var doc = JsonDocument.Parse(responseString);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();
            return text ?? "No response generated.";
        }
        catch
        {
            return "Error parsing Gemini response.";
        }
    }
}
