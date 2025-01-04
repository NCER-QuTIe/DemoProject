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

namespace Demo.Controllers;

[Controller]
[Route("api/admin")]
[ServiceFilter(typeof(ValidationFilterAttribute))]
public class QTITestAdminController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly IQTITestAdminService _service = serviceManager.QTITestAdmin;

    [HttpGet]
    [Route("qtitest/{id:guid}")]
    public async Task<IActionResult> GetQTITest(Guid id) => Ok(await _service.GetQTITestById(id));

    [HttpGet]
    [Route("qtitests")]
    public async Task<IActionResult> GetQTITests()
    {
        var x = await _service.GetQTITests();
        return Ok(x);
    }

    [HttpGet]
    [Route("qtitest/{name}")]
    public async Task<IActionResult> GetQTITestByName(string name) => Ok(await _service.GetQTITestByName(name));

    [HttpPost]
    [Route("qtitest")]
    public async Task<IActionResult> CreateQTITest([FromBody] QTITestCreationDTO qtiTest)
    {
        var createdEntity = await _service.CreateQTITest(qtiTest);
        return CreatedAtAction("GetQTITest", new { id = createdEntity.Id }, createdEntity);
    }

    [HttpDelete]
    [Route("qtitest/{id:guid}")]
    public async Task<IActionResult> DeleteQTITest(Guid id)
    {
        await _service.DeleteQTITestByIdAsync(id);
        return NoContent();
    }

    [HttpPatch]
    [Route("qtitest/{id:guid}")]
    public async Task<IActionResult> UpdateQTITest(Guid id, [FromBody] JsonElement patchObject)
    {
        await _service.UpdateQTITestAsync(id, patchObject);
        return Ok(_service.GetQTITestById(id));
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
