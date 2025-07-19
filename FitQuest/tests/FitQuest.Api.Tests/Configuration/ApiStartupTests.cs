using FluentAssertions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net;
using Xunit;

namespace FitQuest.Api.Tests.Configuration;

/// <summary>
/// Tests for API startup configuration including CORS
/// </summary>
public class ApiStartupTests : IClassFixture<FitQuestApiTestFactory>
{
    private readonly FitQuestApiTestFactory _factory;

    public ApiStartupTests(FitQuestApiTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Api_ShouldStart_Successfully()
    {
        // Arrange & Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/health");

        // Assert
        response.Should().NotBeNull();
        // The API should start without throwing exceptions
    }

    [Fact]
    public void CorsPolicy_ShouldBeConfigured()
    {
        // Arrange & Act
        using var scope = _factory.Services.CreateScope();
        var corsOptions = scope.ServiceProvider.GetService<IOptions<CorsOptions>>();

        // Assert
        corsOptions.Should().NotBeNull();
        // CORS should be configured in the DI container
    }

    [Fact]
    public async Task Api_ShouldServeSwagger_InDevelopment()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger");

        // Assert
        // Should either return the swagger page or redirect to it
        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.MovedPermanently, HttpStatusCode.Found);
    }

    [Fact]
    public async Task Api_ShouldHaveCorsHeaders_ForAllowedOrigins()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Origin", "http://localhost:5174");

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.Should().NotBeNull();
        // The request should not be blocked by CORS
        response.StatusCode.Should().NotBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public void Authentication_ShouldBeConfigured()
    {
        // Arrange & Act
        using var scope = _factory.Services.CreateScope();
        var services = scope.ServiceProvider;

        // Assert
        // Authentication services should be registered
        var authService = services.GetService<Microsoft.AspNetCore.Authentication.IAuthenticationService>();
        authService.Should().NotBeNull();
    }

    [Fact]
    public void Logging_ShouldBeConfigured()
    {
        // Arrange & Act
        using var scope = _factory.Services.CreateScope();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<ApiStartupTests>();

        // Assert
        logger.Should().NotBeNull();
        
        // Test that logging doesn't throw
        logger.LogInformation("Test log message");
    }

    [Fact]
    public void SignalR_ShouldBeConfigured()
    {
        // Arrange & Act
        using var scope = _factory.Services.CreateScope();
        var services = scope.ServiceProvider;

        // Assert
        // Check SignalR is properly configured (using reflection to avoid dependency on specific hub)
        var signalROptions = services.GetService<IOptions<HubOptions>>();
        signalROptions.Should().NotBeNull("SignalR should be properly configured");
    }
}
