using Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Controller]
public class QTITestController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly IQTITestService _service = serviceManager.QTITest;

    [HttpGet]
    [Route("api/qtitest/{id:guid}")]
    public async Task<IActionResult> GetQTITest(Guid id)
    {
        return Ok(await _service.GetQTITestById(id));
    }

    [HttpGet]
    [Route("api/qtitests")]
    public async Task<IActionResult> GetQTITests() => Ok(await _service.GetQTITests());

    [HttpGet]
    [Route("api/qtitest/{name}")]
    public async Task<IActionResult> GetQTITestByName(string name) => Ok(await _service.GetQTITestByName(name));
}