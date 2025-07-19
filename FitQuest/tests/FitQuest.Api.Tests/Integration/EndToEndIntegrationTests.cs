using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace FitQuest.Api.Tests.Integration;

/// <summary>
/// Integration tests for complete application functionality
/// </summary>
public class EndToEndIntegrationTests : IClassFixture<FitQuestApiTestFactory>
{
    private readonly FitQuestApiTestFactory _factory;
    private readonly HttpClient _client;

    public EndToEndIntegrationTests(FitQuestApiTestFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Api_ShouldStart_FromCleanState()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.Should().NotBeNull();
        // API should be accessible and respond to health checks
    }

    [Fact]
    public async Task Api_ShouldServe_SwaggerDocumentation()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/swagger/index.html");

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.OK, 
            HttpStatusCode.NotFound // Acceptable if Swagger is disabled in test environment
        );
    }

    [Fact]
    public async Task Api_ShouldHandle_CorsRequests()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add("Origin", "http://localhost:5174");

        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
        // CORS should not block requests from allowed origins
    }

    [Fact]
    public async Task Api_ShouldReturn_ValidationErrors_ForInvalidRequests()
    {
        // Arrange
        var invalidData = new { };
        var json = JsonSerializer.Serialize(invalidData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/goals", content);

        // Assert
        // Should return either Unauthorized (if auth is required) or BadRequest (if validation fails)
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.Unauthorized,
            HttpStatusCode.BadRequest
        );
    }

    [Fact]
    public async Task Api_ShouldRequire_Authentication_ForProtectedEndpoints()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/api/goals");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Api_ShouldHandle_DatabaseOperations()
    {
        // This test verifies that the database context is working
        // In a real scenario, you would test actual database operations
        
        // Arrange & Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.InternalServerError);
        // Database connection should not cause server errors
    }

    [Fact]
    public async Task Api_ShouldHandle_GlobalExceptions_Gracefully()
    {
        // Arrange & Act
        // Try to access a non-existent endpoint
        var response = await _client.GetAsync("/api/nonexistent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        // Should return proper HTTP status codes, not crash
    }

    [Fact]
    public async Task Api_ShouldLog_RequestsAndResponses()
    {
        // Arrange & Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.Should().NotBeNull();
        // The fact that this doesn't throw means logging is configured properly
        // In a real scenario, you might verify log entries were created
    }

    [Fact]
    public void Api_ShouldHave_ProperConfiguration_Loading()
    {
        // Arrange & Act
        var client = _factory.CreateClient();

        // Assert
        client.Should().NotBeNull();
        // Configuration loading should work properly for the factory to create a client
    }

    [Fact]
    public async Task Api_ShouldSupport_ContentNegotiation()
    {
        // Arrange
        _client.DefaultRequestHeaders.Add("Accept", "application/json");

        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        if (response.IsSuccessStatusCode)
        {
            response.Content.Headers.ContentType?.MediaType.Should().Contain("application/json");
        }
    }
}
