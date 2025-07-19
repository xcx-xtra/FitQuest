using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FitQuest.Api;

namespace FitQuest.Api.Tests;

/// <summary>
/// Custom WebApplicationFactory for testing the FitQuest API
/// </summary>
public class FitQuestApiTestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Set environment to Test first
        builder.UseEnvironment("Test");
        
        // Configure test settings early in the pipeline
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Clear existing configuration providers
            config.Sources.Clear();
            
            // Add test-specific configuration
            config.AddJsonFile("appsettings.Test.json", optional: false)
                  .AddInMemoryCollection(new Dictionary<string, string?>
                  {
                      ["Jwt:Key"] = "test-jwt-key-that-is-at-least-32-characters-long-for-testing-purposes",
                      ["Jwt:Issuer"] = "FitQuest.Api.Tests",
                      ["Jwt:Audience"] = "FitQuest.Client.Tests",
                      ["Jwt:ExpiryInDays"] = "7",
                      ["ConnectionStrings:DefaultConnection"] = "Data Source=:memory:",
                      ["ASPNETCORE_ENVIRONMENT"] = "Test"
                  });
        });

        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<FitQuestContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add in-memory database for testing
            services.AddDbContext<FitQuestContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Build the service provider and create the database
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<FitQuestContext>();
            var logger = scopedServices.GetRequiredService<ILogger<FitQuestApiTestFactory>>();

            try
            {
                // Ensure the database is created
                db.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred creating the test database.");
                throw;
            }
        });

        builder.UseEnvironment("Testing");
    }
}
