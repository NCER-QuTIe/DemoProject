using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Models;

namespace Contracts.Services;

public interface IFeedbackService
{
    public Task<FeedbackDTO> GetFeedbackByIdAsync(Guid id);
    public Task<IEnumerable<FeedbackDTO>> GetFeedbacksAsync();
    public Task<Feedback> CreateFeedbackAsync(FeedbackCreationDTO feedback);
}
