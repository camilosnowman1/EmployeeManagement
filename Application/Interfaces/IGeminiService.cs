namespace Application.Interfaces;

public interface IGeminiService
{
    Task<string> QueryEmployeeDataAsync(string naturalLanguageQuery);
}
