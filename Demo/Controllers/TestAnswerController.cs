﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataTransferObjects.TestAnswer;
using Contracts.Logger;
using MailKit;
using Service;

namespace Demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestAnswerController(ILoggerManager logger, MyMailService mailService) : ControllerBase
{
    [HttpPost]
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
