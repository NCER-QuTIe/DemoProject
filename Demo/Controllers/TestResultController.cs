using Contracts.Logger;
using DataTransferObjects.TestResults;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestResultController(MyMailService mailService) : ControllerBase
{
    [HttpPost()]
    public async Task<IActionResult> SendEmail([FromBody] EmailContentDTO emailContent)
    {
        bool result = await mailService.SendTestResponseMail(emailContent);
        if (result)
        {
            return Ok();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
