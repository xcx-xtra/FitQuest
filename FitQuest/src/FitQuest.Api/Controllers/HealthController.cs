using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitQuest.Api.Services;

namespace FitQuest.Api.Controllers;

/// <summary>
/// Health check controller for monitoring API status
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly FitQuestContext _context;
    private readonly ILogger<HealthController> _logger;
    private readonly IErrorHandlingService _errorHandlingService;

    public HealthController(
        FitQuestContext context, 
        ILogger<HealthController> logger,
        IErrorHandlingService errorHandlingService)
    {
        _context = context;
        _logger = logger;
        _errorHandlingService = errorHandlingService;
    }

    /// <summary>
    /// Basic health check endpoint
    /// </summary>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { 
            status = "healthy", 
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
        });
    }

    /// <summary>
    /// Detailed health check including database connectivity
    /// </summary>
    [HttpGet("detailed")]
    public async Task<IActionResult> GetDetailed()
    {
        var healthStatus = new
        {
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
            checks = new Dictionary<string, object>()
        };

        // Check database connectivity
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            healthStatus.checks["database"] = new
            {
                status = canConnect ? "healthy" : "unhealthy",
                responseTime = await MeasureDatabaseResponseTime()
            };

            if (canConnect)
            {
                _logger.LogInformation("Health check: Database connection successful");
            }
            else
            {
                _logger.LogWarning("Health check: Database connection failed");
            }
        }
        catch (Exception ex)
        {
            var friendlyMessage = _errorHandlingService.LogAndGetUserFriendlyMessage(ex, _logger, "Health check database connectivity");
            healthStatus.checks["database"] = new
            {
                status = "unhealthy",
                error = friendlyMessage
            };
        }

        // Determine overall status
        var overallHealthy = healthStatus.checks.Values
            .Cast<dynamic>()
            .All(check => check.status == "healthy");

        var result = new
        {
            status = overallHealthy ? "healthy" : "unhealthy",
            timestamp = healthStatus.timestamp,
            version = healthStatus.version,
            environment = healthStatus.environment,
            checks = healthStatus.checks
        };

        return overallHealthy ? Ok(result) : StatusCode(503, result);
    }

    private async Task<double> MeasureDatabaseResponseTime()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SELECT 1");
            stopwatch.Stop();
            return stopwatch.Elapsed.TotalMilliseconds;
        }
        catch
        {
            stopwatch.Stop();
            return -1; // Indicates failure
        }
    }
}