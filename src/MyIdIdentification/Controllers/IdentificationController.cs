using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAsync(string code, long userId, long orgId)
    {
        var myIdSdkResponse = await _myIdService.GetMyIdSdkResponseAsync(code, userId, orgId);

        return Ok(myIdSdkResponse);
    }
}