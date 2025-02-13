using Contracts.Services;
using DataTransferObjects.Creation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers;

[Route("api")]
[ApiController]
public class FeedbackController(IServiceManager serviceManager) : ControllerBase
{
    [HttpGet]
    [Route("feedbacks")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public async Task<IActionResult> GetAllFeedbacksAsync()
    {
        var feedbacks = await serviceManager.Feedback.GetFeedbacksAsync();
        return Ok(feedbacks);
    }

    [HttpGet("feedback/{id:guid}", Name = "GetFeedbackById")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public async Task<IActionResult> GetFeedbackByIdAsync(Guid id)
    {
        var feedback = await serviceManager.Feedback.GetFeedbackByIdAsync(id);
        return Ok(feedback);
    }

    [HttpPost("feedback")]
    public async Task<IActionResult> CreateFeedbackAsync([FromBody] FeedbackCreationDTO feedback)
    {
        var createdEntity = await serviceManager.Feedback.CreateFeedbackAsync(feedback);
        return CreatedAtRoute("GetFeedbackById", new { id = createdEntity.Id }, createdEntity);
    }

    [HttpDelete("feedback/{id:guid}")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public async Task<IActionResult> DeleteFeedbackAsync(Guid id)
    {
        await serviceManager.Feedback.DeleteFeedbackByIdAsync(id);
        return NoContent();
    }
}
