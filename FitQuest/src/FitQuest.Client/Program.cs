using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FitQuest.Client;
using Microsoft.AspNetCore.Components.Authorization;
using FitQuest.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IMockAuthService, MockAuthService>();
builder.Services.AddScoped<ILoginService, MockAuthService>();
builder.Services.AddScoped<GoalService>();
builder.Services.AddScoped<PointService>();

await builder.Build().RunAsync();
