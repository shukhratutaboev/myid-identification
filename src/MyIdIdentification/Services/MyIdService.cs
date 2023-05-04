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


    public async Task<MyIdSdkModel> GetMyIdSdkModelAsync(string code, long userId, string method, EProviderType providerType)
    {
        var provider = await _context.Providers.FirstOrDefaultAsync(p => p.Method == method && p.ProviderType == providerType);
        if (provider == null || provider.Credentials == null)
        {
            _logger.LogError($"Credentials doesn't exist for given provider.");
            throw new Exception($"Credentials doesn't exist for given provider.");
        }

        var accessToken = await GetMyIdSdkAccessTokenAsync(code, provider);

        var request = new HttpRequestMessage(HttpMethod.Get, _options.GetMe);

        request.Headers.Authorization = new AuthenticationHeaderValue(accessToken.TokenType, accessToken.AccessToken);

        var model = await _httpClient.SendAsync(request);

        if (!model.IsSuccessStatusCode)
        {
            var error = await model.Content.ReadAsStringAsync();
            _logger.LogError("Error while getting user info from MyId");
            throw new Exception($"Error while getting user info from MyId: {error}");
        }

        var modelString = await model.Content.ReadAsStringAsync();
        var myIdSdkModel = JsonSerializer.Deserialize<MyIdSdkModel>(modelString);
        var doc = ToDocument(myIdSdkModel);
        doc.UserId = userId;
        doc.ProviderId = provider.Id;
        await _context.Passports.AddAsync(doc);
        await _context.SaveChangesAsync();
        return myIdSdkModel;
    }

    public async Task<MyIdSdkModel> GetConfirmedUserAsync(long userId, string method, EProviderType providerType)
    {
        var user = await _context.Passports.FirstOrDefaultAsync(d => d.UserId == userId && d.Provider.Method == method && d.Provider.ProviderType == providerType);

        if (user == null)
        {
            _logger.LogError($"User with id {userId} doesn't exist.");
            return null;
        }

        return user.AllData.Deserialize<MyIdSdkModel>();
    }

    private async Task<MyIdAccessTokenModel> GetMyIdSdkAccessTokenAsync(string code, Provider provider)
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

        var model = await _httpClient.SendAsync(request);

        if (!model.IsSuccessStatusCode)
        {
            var error = await model.Content.ReadAsStringAsync();
            _logger.LogError("Error while getting access token from MyId");
            throw new Exception($"Error while getting access token from MyId: {error}");
        }

        var modelString = await model.Content.ReadAsStringAsync();
        var accessTokenModel = JsonSerializer.Deserialize<MyIdAccessTokenModel>(modelString);
        return accessTokenModel;
    }

    public Passport ToDocument(MyIdSdkModel model)
        => new Passport()
        {
            FirstName = model.Profile.CommonData.FirstName,
            LastName = model.Profile.CommonData.LastName,
            MiddleName = model.Profile.CommonData.MiddleName,
            BirthDate = DateTime.ParseExact(model.Profile.CommonData.BirthDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            Tin = model.Profile.CommonData.Inn,
            Pinfl = model.Profile.CommonData.Pinfl,
            PassportNumber = model.Profile.DocData.PassData[2..],
            PassportSerial = model.Profile.DocData.PassData[..2],
            PassportGivenDate = DateTime.ParseExact(model.Profile.DocData.IssuedDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            PassportExpireDate = DateTime.ParseExact(model.Profile.DocData.ExpiryDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
            PassportGivenBy = model.Profile.DocData.IssuedBy,
            Address = model.Profile.Address.PermanentAddress,
            ProviderType = EProviderType.MyId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            AllData = JsonDocument.Parse(JsonSerializer.Serialize(model))
        };
}