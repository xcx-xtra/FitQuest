using Serilog;
using Serilog.Events;

namespace FitQuest.Api.Configuration;

public static class LoggingConfiguration
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        // Configure Serilog for structured logging
        var loggerConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "FitQuest.Api")
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName);

        // Configure different sinks based on environment
        if (builder.Environment.IsDevelopment())
        {
            loggerConfig
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Debug)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authorization", LogEventLevel.Debug)
                .WriteTo.Console(outputTemplate: 
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/fitquest-dev-.log", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj} {Properties:j}{NewLine}{Exception}");
        }
        else
        {
            loggerConfig
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console(outputTemplate: 
                    "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/fitquest-.log", 
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {SourceContext}: {Message:lj} {Properties:j}{NewLine}{Exception}");
        }

        // Create the logger
        Log.Logger = loggerConfig.CreateLogger();

        // Use Serilog for the application
        builder.Host.UseSerilog();

        // Add custom log enrichment for request context
        builder.Services.Configure<LoggerFilterOptions>(options =>
        {
            // Filter out noisy logs that aren't useful for debugging
            options.AddFilter("Microsoft.AspNetCore.Hosting.Diagnostics", LogLevel.Warning);
            options.AddFilter("Microsoft.AspNetCore.Mvc.Infrastructure", LogLevel.Warning);
            options.AddFilter("Microsoft.AspNetCore.Routing.EndpointMiddleware", LogLevel.Warning);
        });
    }

    public static void LogApplicationStartup(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("=== FitQuest API Starting ===");
        logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
        logger.LogInformation("Content Root: {ContentRoot}", app.Environment.ContentRootPath);
        
        if (app.Environment.IsDevelopment())
        {
            logger.LogInformation("Development mode: Enhanced logging and error details enabled");
        }
    }

    public static void LogApplicationShutdown(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("=== FitQuest API Shutting Down ===");
    }
}