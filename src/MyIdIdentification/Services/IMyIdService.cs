using MyIdIdentification.Models;

namespace MyIdIdentification.Services;

public interface IMyIdService
{
    Task<MyIdSdkResponse> GetMyIdSdkResponseAsync(string code, long userId, long orgId);
}