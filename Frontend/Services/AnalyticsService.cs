using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Frontend.Models.Analytics;
using Microsoft.JSInterop;
using Microsoft.Extensions.Logging;

namespace Frontend.Services;

public class AnalyticsService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(
        IHttpClientFactory factory,
        ILocalStorageService localStorage,
        ILogger<AnalyticsService> logger)
    {
        _httpClient = factory.CreateClient("AnalyticsClient");
        _localStorage = localStorage;
        _logger = logger;
    }
    public async Task<DashboardDto?> GetDashboard(int memberId)
    {
        _logger.LogInformation("Fetching dashboard for member {MemberId}", memberId);
        try
        {
            var dashboard = await _httpClient.GetFromJsonAsync<DashboardDto>(
                $"members/{memberId}/dashboard");
            _logger.LogInformation("Successfully fetched dashboard for member {MemberId}", memberId);
            return dashboard;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard for member {MemberId}", memberId);
            throw;
        }
    }
}