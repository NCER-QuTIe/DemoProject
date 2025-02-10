using Contracts.Services;
using DataTransferObjects.Creation;
using Contracts.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using LoggerService;
using System.Runtime.CompilerServices;
using Entities.Models;
using Entities.Enums;
using Demo.ActionFilters;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers;

[Controller]
[Route("api/admin")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
[Authorize(AuthenticationSchemes = "BasicAuthentication")]
public class ExternalTestAdminController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly IExternalTestAdminService _service = serviceManager.ExternalTestAdmin;

    [HttpGet]
    [Route("externaltest/{id:guid}")]
    public async Task<IActionResult> GetExternalTest(Guid id) => Ok(await _service.GetExternalTestById(id));

    [HttpGet]
    [Route("externaltests")]
    public async Task<IActionResult> GetExternalTests()
    {
        var x = await _service.GetExternalTests();
        return Ok(x);
    }

    [HttpGet]
    [Route("externaltest/{name}")]
    public async Task<IActionResult> GetExternalTestByName(string name) => Ok(await _service.GetExternalTestByName(name));

    [HttpPost]
    [Route("externaltest")]
    public async Task<IActionResult> CreateExternalTest([FromBody] ExternalTestCreationDTO externalTest)
    {
        var createdEntity = await _service.CreateExternalTest(externalTest);
        return CreatedAtAction("GetExternalTest", new { id = createdEntity.Id }, createdEntity);
    }

    [HttpDelete]
    [Route("externaltest/{id:guid}")]
    public async Task<IActionResult> DeleteExternalTest(Guid id)
    {
        await _service.DeleteExternalTestByIdAsync(id);
        return NoContent();
    }

    [HttpPatch]
    [Route("externaltest/{id:guid}")]
    public async Task<IActionResult> UpdateExternalTest(Guid id, [FromBody] JsonElement patchObject)
    {
        await _service.UpdateExternalTestAsync(id, patchObject);
        return Ok(_service.GetExternalTestById(id));
    }

    //[HttpDelete]
    //[Route("qtitests")]
    //public async Task<IActionResult> DeleteQTITest()
    //{
    //    var tests = await _service.GetQTITests();
    //    foreach (var test in tests)
    //    {
    //        await _service.DeleteQTITestByIdAsync(test.Id);
    //    }
    //    return Ok();
    //}
}
