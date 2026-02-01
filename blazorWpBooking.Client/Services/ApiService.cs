using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(navigationManager.BaseUri);
    }

    public async Task<T?> GetAsync<T>(string url, CancellationToken cancellationToken = default( CancellationToken ) )
    {
        return await _httpClient.GetFromJsonAsync<T>(url, cancellationToken);
    }
}