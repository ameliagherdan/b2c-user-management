using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using UserManagementSystem.Application.DTOs;
using UserManagementSystem.Application.Interfaces;

namespace UserManagementSystem.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public AuthService(IConfiguration config)
    {
        _config = config;
        _httpClient = new HttpClient();
    }

    public async Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginRequest request)
    {
        var domain = _config["AzureAdB2C:Domain"];
        var policy = _config["AzureAdB2C:SignUpSignInPolicyId"];
        var clientId = _config["AzureAdB2C:ClientId"];
        var clientSecret = _config["AzureAdB2C:ClientSecret"];
        var scope = _config["AzureAdB2C:Scope"];

        var tokenEndpoint = $"https://{domain}/oauth2/v2.0/token?p={policy}";

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "password"),
            new KeyValuePair<string, string>("username", request.Email),
            new KeyValuePair<string, string>("password", request.Password),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("scope", $"{scope} offline_access openid")
        });

        var response = await _httpClient.PostAsync(tokenEndpoint, content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return (false, null, responseBody);
        }

        return (true, responseBody, null); // you could parse and return just access_token if needed
    }
}