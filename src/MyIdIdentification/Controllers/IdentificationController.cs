using Microsoft.AspNetCore.Mvc;
using MyIdIdentification.Entities.Enums;
using MyIdIdentification.Services;

namespace MyIdIdentification.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentificationController : ControllerBase
{
    private readonly ILogger<IdentificationController> _logger;
    private readonly IMyIdService _myIdService;

    public IdentificationController(
        ILogger<IdentificationController> logger,
        IMyIdService myIdService)
    {
        _logger = logger;
        _myIdService = myIdService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(string code, long userId, string method)
    {
        var myIdSdkModel = await _myIdService.GetMyIdSdkModelAsync(code, userId, method, EProviderType.MyId);

        return Ok(myIdSdkModel);
    }
}