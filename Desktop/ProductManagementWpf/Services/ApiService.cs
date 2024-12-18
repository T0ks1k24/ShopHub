using Newtonsoft.Json;
using ProductManagementWpf.Models;
using ProductManagementWpf.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProductManagementWpf.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient ;
    public ApiService(HttpClient httpClient)
    {
        // Initialization HttpClient
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7113/");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}api/auth/login", loginRequest);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new ApiException(errorMessage, (int)response.StatusCode);
        }

        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}api/auth/register", registerRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthResponse>();
    }
}

