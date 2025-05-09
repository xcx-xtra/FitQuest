using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FitQuest.Client;
using Microsoft.AspNetCore.Components.Authorization;
using FitQuest.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5124/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<PointService>();
builder.Services.AddScoped<LeaderboardService>();

await builder.Build().RunAsync();
