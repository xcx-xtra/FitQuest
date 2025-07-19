using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using FitQuest.Shared.Models;

namespace FitQuest.Client.Services
{
    public class PointService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authProvider;

        public PointService(HttpClient http, AuthenticationStateProvider authProvider)
        {
            _http = http;
            _authProvider = authProvider;
        }

        public async Task<PointSummaryDto?> GetUserPointsAsync(string userId)
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var token = authState.User.FindFirst(c => c.Type == "access_token")?.Value;

            if (token != null)
            {
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await _http.GetFromJsonAsync<PointSummaryDto>($"api/users/{userId}/points");
        }
    }
}