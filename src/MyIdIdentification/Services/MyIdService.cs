using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MyIdIdentification.Context;
using MyIdIdentification.Entities;
using MyIdIdentification.Entities.Enums;
using MyIdIdentification.Models;
using MyIdIdentification.Options;

namespace MyIdIdentification.Services;

public class MyIdService : IMyIdService
{
    private readonly ILogger<MyIdService> _logger;
    private readonly IdentificationContext _context;
    private readonly Urls _options;
    private readonly HttpClient _httpClient;

    public MyIdService(
        ILogger<MyIdService> logger,
        IdentificationContext context,
        IOptions<Urls> options,
        HttpClient httpClient)
    {
        _logger = logger;
        _context = context;
        _options = options.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_options.BaseUrl);
    }


    public async Task<MyIdSdkResponse> GetMyIdSdkResponseAsync(string code, long userId, long orgId)
    {
        var provider = await _context.Providers.FirstOrDefaultAsync(p => p.OrganizationId == orgId && p.IdentificationType == EIdentificationType.MyId);
        if (provider == null || provider.Credentials == null)
        {
            _logger.LogError($"Credentials doesn't exist for {orgId} organization.");
            throw new Exception($"Credentials doesn't exist for {orgId} organization.");
        }

        var accessToken = await GetMyIdSdkAccessTokenAsync(code, orgId, provider);

        var request = new HttpRequestMessage(HttpMethod.Get, _options.GetMe);

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
        var doc = ToDocument(myIdSdkResponse);
        doc.UserId = userId;
        doc.ProviderId = provider.Id;
        await _context.Documents.AddAsync(doc);
        await _context.SaveChangesAsync();
        return myIdSdkResponse;
    }

    private async Task<MyIdAccessTokenResponse> GetMyIdSdkAccessTokenAsync(string code, long orgId, Provider provider)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, _options.GetAccessToken);

        var credentials = provider.Credentials.Deserialize<Security>();

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            {"grant_type", credentials.GrantTypeSdk},
            {"code", code},
            {"client_id", credentials.ClientIdSdk},
            {"client_secret", credentials.ClientSecret}
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

    public Document ToDocument(MyIdSdkResponse response)
        => new Document()
        {
            FirstName = response.Profile.CommonData.FirstName,
            LastName = response.Profile.CommonData.LastName,
            MiddleName = response.Profile.CommonData.MiddleName,
            BirthDate = DateTime.ParseExact(response.Profile.CommonData.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            Tin = response.Profile.CommonData.Inn,
            Pinfl = response.Profile.CommonData.Pinfl,
            PassportNumber = response.Profile.DocData.PassData[2..],
            PassportSerial = response.Profile.DocData.PassData[..2],
            PassportGivenDate = DateTime.ParseExact(response.Profile.DocData.IssuedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            PassportExpireDate = DateTime.ParseExact(response.Profile.DocData.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            PassportGivenBy = response.Profile.DocData.IssuedBy,
            Address = response.Profile.Address.PermanentAddress,
            IdentificationType = EIdentificationType.MyId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AllData = JsonDocument.Parse(JsonSerializer.Serialize(response))
        };
}