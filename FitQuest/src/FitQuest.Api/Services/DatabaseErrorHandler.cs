using Microsoft.EntityFrameworkCore;

namespace FitQuest.Api.Services;

public interface IDatabaseErrorHandler
{
    Task<bool> HandleDatabaseConnectionAsync(FitQuestContext context, ILogger logger);
    string GetFriendlyErrorMessage(Exception exception);
    Task<bool> ValidateDatabaseSchemaAsync(FitQuestContext context, ILogger logger);
}

public class DatabaseErrorHandler : IDatabaseErrorHandler
{
    private readonly IWebHostEnvironment _environment;

    public DatabaseErrorHandler(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<bool> HandleDatabaseConnectionAsync(FitQuestContext context, ILogger logger)
    {
        try
        {
            // Test database connection with timeout
            using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            await context.Database.CanConnectAsync(cancellationTokenSource.Token);
            
            logger.LogInformation("Database connection established successfully");
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database connection failed");
            
            var friendlyMessage = GetFriendlyErrorMessage(ex);
            logger.LogError("Database Error: {FriendlyMessage}", friendlyMessage);
            
            if (_environment.IsDevelopment())
            {
                logger.LogError("Technical Details: {TechnicalDetails}", ex.Message);
                
                // Provide specific guidance for common issues
                if (ex.Message.Contains("SQLite"))
                {
                    logger.LogInformation("SQLite Troubleshooting Guide:");
                    logger.LogInformation("- Ensure the application has write permissions to the database directory");
                    logger.LogInformation("- Check if the database file path is accessible: {DatabasePath}", GetDatabasePath(context));
                    logger.LogInformation("- Verify SQLite provider is properly configured");
                    logger.LogInformation("- Check if the directory exists and is writable");
                }
                else if (ex.Message.Contains("network") || ex.Message.Contains("connection"))
                {
                    logger.LogInformation("Connection Troubleshooting Guide:");
                    logger.LogInformation("- Check your connection string configuration");
                    logger.LogInformation("- Ensure the database server is running");
                    logger.LogInformation("- Verify network connectivity to the database");
                    logger.LogInformation("- Check firewall settings");
                }
            }
            
            return false;
        }
    }

    public async Task<bool> ValidateDatabaseSchemaAsync(FitQuestContext context, ILogger logger)
    {
        try
        {
            // Check if database exists and has expected tables
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();
            
            logger.LogInformation("Database schema validation: {AppliedCount} applied migrations, {PendingCount} pending migrations", 
                appliedMigrations.Count(), pendingMigrations.Count());
            
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database schema validation failed");
            var friendlyMessage = GetFriendlyErrorMessage(ex);
            logger.LogError("Schema Validation Error: {FriendlyMessage}", friendlyMessage);
            return false;
        }
    }

    public string GetFriendlyErrorMessage(Exception exception)
    {
        return exception switch
        {
            InvalidOperationException when exception.Message.Contains("SQLite") => 
                "SQLite database configuration error. Check file permissions and path.",
            
            InvalidOperationException when exception.Message.Contains("connection") => 
                "Database connection configuration error. Check your connection string.",
            
            TimeoutException => 
                "Database operation timed out. The database may be busy or unreachable.",
            
            UnauthorizedAccessException => 
                "Database access denied. Check authentication credentials and permissions.",
            
            DbUpdateException => 
                "Database update failed. This may be due to data validation or constraint violations.",
            
            DirectoryNotFoundException => 
                "Database directory not found. Check if the path exists and is accessible.",
            
            _ => _environment.IsDevelopment() 
                ? $"Database error: {exception.Message}" 
                : "A database error occurred. Please try again later."
        };
    }

    private string GetDatabasePath(FitQuestContext context)
    {
        try
        {
            var connectionString = context.Database.GetConnectionString();
            if (connectionString?.Contains("Data Source=") == true)
            {
                var startIndex = connectionString.IndexOf("Data Source=") + "Data Source=".Length;
                var endIndex = connectionString.IndexOf(';', startIndex);
                if (endIndex == -1) endIndex = connectionString.Length;
                return connectionString.Substring(startIndex, endIndex - startIndex);
            }
            return "Unknown";
        }
        catch
        {
            return "Unable to determine database path";
        }
    }
}