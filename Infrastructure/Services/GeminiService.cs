using Application.Interfaces;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text.Json;

namespace Infrastructure.Services;

public class GeminiService : IGeminiService
{
    private readonly IEmployeeRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public GeminiService(IEmployeeRepository repository, IConfiguration configuration, HttpClient httpClient)
    {
        _repository = repository;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> QueryEmployeeDataAsync(string naturalLanguageQuery)
    {
        var apiKey = _configuration["GeminiApiKey"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        
        if (string.IsNullOrEmpty(apiKey))
        {
            return "Gemini API Key not configured. Please set GEMINI_API_KEY environment variable.";
        }

        // Get all employees for context
        var employees = await _repository.GetAllAsync();
        var employeeData = System.Text.Json.JsonSerializer.Serialize(employees.Select(e => new
        {
            e.JobTitle,
            e.Department,
            e.Status,
            e.EducationLevel
        }));

        // Build prompt for Gemini
        var prompt = $@"You are a data analyst. Based on this employee data:
{employeeData}

Answer this question concisely in one sentence: {naturalLanguageQuery}

Provide only the numerical answer and a brief explanation. Do not make up data.";

        try
        {
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

            var response = await _httpClient.PostAsJsonAsync(
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}",
                requestBody
            );

            if (!response.IsSuccessStatusCode)
            {
                return $"Error calling Gemini API: {response.StatusCode}";
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();
            var text = result
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "No response from Gemini.";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
