using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FitQuest.Client;
using Microsoft.AspNetCore.Components.Authorization;
using FitQuest.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure logging for better error tracking
builder.Logging.SetMinimumLevel(LogLevel.Information);
if (builder.HostEnvironment.IsDevelopment())
{
    builder.Logging.AddFilter("Microsoft.AspNetCore.Components.WebAssembly", LogLevel.Information);
}

// Configure HttpClient with base address from configuration
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5124/";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

// Add error handling and notification services
builder.Services.AddSingleton<IToastService, ToastService>();
builder.Services.AddScoped<ISignalRService, SignalRService>();
builder.Services.AddScoped<IErrorHandlerService, ErrorHandlerService>();

// Configure authentication and authorization
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Add application services
builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<PointService>();
builder.Services.AddScoped<LeaderboardService>();

var app = builder.Build();

// Don't initialize SignalR during startup - let components initialize it when needed
// This prevents DOM attachment issues during app startup

await app.RunAsync();
