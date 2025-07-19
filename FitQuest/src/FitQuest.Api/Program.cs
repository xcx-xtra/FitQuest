using FitQuest.Api;
using FitQuest.Api.Configuration;
using FitQuest.Api.Middleware;
using FitQuest.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using User = FitQuest.Shared.Models.User;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure structured logging with Serilog for better observability
builder.ConfigureLogging();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure SQLite database for development with automatic migrations
builder.Services.AddDbContext<FitQuestContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Default"));
});

// Configure ASP.NET Core Identity for user management
// Uses custom User model with integer primary keys and Entity Framework stores
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<FitQuestContext>()
    .AddDefaultTokenProviders();

// Validate JWT configuration at startup to fail fast if misconfigured
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");

// Configure JWT Bearer authentication for API protection
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts => {
    // Token validation parameters ensure only valid JWTs are accepted
    opts.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
    
    // Configure JWT Bearer authentication for SignalR connections
    // Allows JWT tokens to be passed via query string for WebSocket connections
    opts.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/leaderboardHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Configure CORS for cross-origin requests from the Blazor client
var allowedOrigins = builder.Configuration.GetSection("Api:AllowedOrigins").Get<string[]>() ?? ["http://localhost:5174"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCorsPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Required for SignalR and authentication
    });
});

// Register core application services
builder.Services.AddControllers();
builder.Services.AddSignalR(); // Real-time communication for leaderboards and notifications

// Register background services for scheduled tasks
builder.Services.AddHostedService<DailyGoalResetService>(); // Resets daily goals at midnight
builder.Services.AddHostedService<ScheduledTaskService>(); // Handles recurring maintenance tasks
builder.Services.AddSingleton<IEmailSender, MockEmailSender>(); // Email service for notifications

// Register custom error handling services for comprehensive error management
builder.Services.AddScoped<IDatabaseErrorHandler, DatabaseErrorHandler>();
builder.Services.AddScoped<IErrorHandlingService, ErrorHandlingService>();

var app = builder.Build();

// Log application startup information for debugging and monitoring
app.LogApplicationStartup();

// Add Serilog request logging (before exception handling)
app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.GetLevel = (httpContext, elapsed, ex) => ex != null 
        ? LogEventLevel.Error 
        : httpContext.Response.StatusCode > 499 
            ? LogEventLevel.Error 
            : LogEventLevel.Information;
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.FirstOrDefault());
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
});

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

// Automatically apply database migrations and handle database connection failures
var autoMigrate = builder.Configuration.GetValue<bool>("Database:AutoMigrate", true);
if (autoMigrate && app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<FitQuestContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var dbErrorHandler = scope.ServiceProvider.GetRequiredService<IDatabaseErrorHandler>();
        
        // Use the database error handler for better error management
        var connectionSuccessful = await dbErrorHandler.HandleDatabaseConnectionAsync(context, logger);
        
        if (connectionSuccessful)
        {
            try
            {
                // Apply pending migrations
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    logger.LogInformation("Applying {Count} pending database migrations...", pendingMigrations.Count());
                    await context.Database.MigrateAsync();
                    logger.LogInformation("Database migrations applied successfully.");
                }
                else
                {
                    logger.LogInformation("Database is up to date. No migrations needed.");
                }
                
                // Seed data if configured
                var seedData = builder.Configuration.GetValue<bool>("Database:SeedData", false);
                if (seedData)
                {
                    logger.LogInformation("Seeding development data...");
                    // Add seed data logic here if needed in the future
                    logger.LogInformation("Development data seeding completed.");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Database migration failed");
                var friendlyMessage = dbErrorHandler.GetFriendlyErrorMessage(ex);
                logger.LogError("Migration Error: {FriendlyMessage}", friendlyMessage);
                
                // Don't throw in development to allow the app to start for debugging
                logger.LogWarning("Application will continue to start, but database operations may fail.");
            }
        }
        else
        {
            logger.LogWarning("Database connection failed. Application will start but database operations may not work.");
        }
    }
}

// Apply CORS before other middleware
app.UseCors("DevelopmentCorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var enableSwagger = builder.Configuration.GetValue<bool>("Api:EnableSwagger", true);
    if (enableSwagger)
    {
        app.MapOpenApi();
    }
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// Add a default route to handle requests to the root URL
app.MapGet("/", () => Results.Ok("Welcome to the FitQuest API!"));

// Add health check endpoint for testing
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

// Map controllers to enable attribute routing
app.MapControllers();
app.MapHub<LeaderboardHub>("/leaderboardHub");

// Configure graceful shutdown
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() => app.LogApplicationShutdown());

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Make Program class public for testing
public partial class Program { }
