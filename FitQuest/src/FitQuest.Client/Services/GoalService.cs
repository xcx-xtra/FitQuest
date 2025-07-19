using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using FitQuest.Shared.Models;

public class GoalService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;

    public GoalService(HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> SubmitGoalAsync(DailyGoal goal)
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var token = authState.User.FindFirst("access_token")?.Value;

        if (token != null)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PostAsJsonAsync("api/goals", goal);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<DailyGoal>> GetUserGoalsAsync(string userId)
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var token = authState.User.FindFirst("access_token")?.Value;

        if (token != null)
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await _http.GetFromJsonAsync<List<DailyGoal>>($"api/goals/user/{userId}") ?? new List<DailyGoal>();
    }
}