using MyIdIdentification.Entities.Enums;
using MyIdIdentification.Models;
using MyIdIdentification.Services;

namespace MyIdIdentification.Managers;

public class MyIdManager
{
    private readonly ILogger<MyIdManager> _logger;
    private readonly IMyIdService _myIdService;

    public MyIdManager(
        ILogger<MyIdManager> logger,
        IMyIdService myIdService)
    {
        _logger = logger;
        _myIdService = myIdService;
    }

    public async Task<MyIdSdkModel> GetMyIdSdkModelAsync(string code, long userId, string method, EProviderType providerType)
    {
        var myIdSdkModel = await _myIdService.GetMyIdSdkModelAsync(code, userId, method, providerType);

        return myIdSdkModel;
    }

    public async Task<MyIdSdkModel> GetConfirmedUserAsync(long userId, string method, EProviderType providerType)
    {
        var myIdSdkModel = await _myIdService.GetConfirmedUserAsync(userId, method, providerType);

        return myIdSdkModel;
    }
}