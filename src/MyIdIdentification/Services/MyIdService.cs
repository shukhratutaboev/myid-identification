using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyIdIdentification.Models;
using MyIdIdentification.Options;

namespace MyIdIdentification.Services;

public class MyIdService : IMyIdService
{
    private readonly ILogger<MyIdService> _logger;
    private readonly MyIdOptions _options;
    private readonly HttpClient _httpClient;

    public MyIdService(
        ILogger<MyIdService> logger,
        IOptions<MyIdOptions> options,
        HttpClient httpClient)
    {
        _logger = logger;
        _options = options.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_options.Urls.BaseUrl);
    }


    public async Task<MyIdSdkResponse> GetMyIdSdkResponseAsync(string code)
    {
        var accessToken = await GetMyIdSdkAccessTokenAsync(code);

        var request = new HttpRequestMessage(HttpMethod.Get, _options.Urls.GetMe);

        request.Headers.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.AccessToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error while getting user info from MyId");
            throw new Exception($"Error while getting user info from MyId: {error}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var myIdSdkResponse = JsonSerializer.Deserialize<MyIdSdkResponse>(responseString);
        return myIdSdkResponse;
    }

    private async Task<MyIdAccessTokenResponse> GetMyIdSdkAccessTokenAsync(string code)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _options.Urls.GetAccessToken);

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", _options.Security.GrantTypeSdk},
            {"code", code},
            {"client_id", _options.Security.ClientIdSdk},
            {"client_secret", _options.Security.ClientSecret}
        });

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error while getting access token from MyId");
            throw new Exception($"Error while getting access token from MyId: {error}");
        }

        var responseString = await response.Content.ReadAsStringAsync();
        var accessTokenResponse = JsonSerializer.Deserialize<MyIdAccessTokenResponse>(responseString);
        return accessTokenResponse;
    }
}