using MyIdIdentification.Entities.Enums;
using MyIdIdentification.Models;

namespace MyIdIdentification.Services;

public interface IMyIdService
{
    Task<MyIdSdkModel> GetMyIdSdkModelAsync(string code, long userId, string method, EProviderType providerType);
    Task<MyIdSdkModel> GetConfirmedUserAsync(long userId, string method, EProviderType providerType);
}