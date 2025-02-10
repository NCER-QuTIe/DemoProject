using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Controller]
public class ExternalTestController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly IExternalTestService _service = serviceManager.ExternalTest;

    [HttpGet]
    [Route("api/Externaltest/{id:guid}")]
    public async Task<IActionResult> GetExternalTest(Guid id)
    {
        return Ok(await _service.GetExternalTestById(id));
    }

    [HttpGet]
    [Route("api/Externaltests")]
    public async Task<IActionResult> GetExternalTests() => Ok(await _service.GetExternalTests());

    [HttpGet]
    [Route("api/Externaltest/{name}")]
    public async Task<IActionResult> GetExternalTestByName(string name) => Ok(await _service.GetExternalTestByName(name));
}