using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EmployeeManagement.Tests.IntegrationTests;

public class DepartmentsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DepartmentsApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetDepartments_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/departments");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetDepartments_ReturnsListOfStrings()
    {
        // Act
        var response = await _client.GetAsync("/api/departments");
        var departments = await response.Content.ReadFromJsonAsync<List<string>>();

        // Assert
        Assert.NotNull(departments);
        Assert.IsType<List<string>>(departments);
    }
}
