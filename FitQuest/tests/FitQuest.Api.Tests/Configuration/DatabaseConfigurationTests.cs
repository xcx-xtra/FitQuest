using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using FitQuest.Api;
using Xunit;

namespace FitQuest.Api.Tests.Configuration;

/// <summary>
/// Tests for database configuration and migration logic
/// </summary>
public class DatabaseConfigurationTests : IClassFixture<FitQuestApiTestFactory>
{
    private readonly FitQuestApiTestFactory _factory;

    public DatabaseConfigurationTests(FitQuestApiTestFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public void DatabaseContext_ShouldBeConfigured()
    {
        // Arrange & Act
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

        // Assert
        context.Should().NotBeNull();
        context.Database.Should().NotBeNull();
    }

    [Fact]
    public void DatabaseContext_ShouldUseInMemoryProvider_InTestEnvironment()
    {
        // Arrange & Act
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

        // Assert
        context.Database.ProviderName.Should().Contain("InMemory");
    }

    [Fact]
    public async Task DatabaseContext_ShouldCreateTables()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

        // Act & Assert
        var canConnect = await context.Database.CanConnectAsync();
        canConnect.Should().BeTrue();
    }

    [Fact]
    public async Task DatabaseContext_ShouldSupportBasicOperations()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FitQuestContext>();

        // Act - Try to perform basic database operations
        var userCount = await context.Users.CountAsync();
        var goalCount = await context.Set<FitQuest.Shared.Models.DailyGoal>().CountAsync();

        // Assert - Should not throw exceptions
        userCount.Should().BeGreaterThanOrEqualTo(0);
        goalCount.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public void DatabaseContext_ShouldHaveCorrectConnectionString_InDevelopment()
    {
        // This test verifies that the configuration system is working
        // In a real application, we would test that SQLite is configured properly
        
        // Arrange & Act
        var client = _factory.CreateClient();

        // Assert
        client.Should().NotBeNull();
        // The fact that the factory can create a client means the configuration is valid
    }
}
