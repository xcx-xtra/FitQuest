using Microsoft.JSInterop;

public class TokenService : ITokenService
{
    private readonly IJSRuntime js;
    private const string TokenKey = "fitquest-token";

    public TokenService(IJSRuntime js) => this.js = js;

    public async Task SetTokenAsync(string token) => await js.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
    public async Task<string?> GetTokenAsync() => await js.InvokeAsync<string>("localStorage.getItem", TokenKey);
    public async Task ClearTokenAsync() => await js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
}
