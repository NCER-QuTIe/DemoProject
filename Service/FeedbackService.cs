using Amazon.Runtime.Internal.Auth;
using AutoMapper;
using Contracts.Logger;
using Contracts.Repositories;
using Contracts.Services;
using DataTransferObjects.Creation;
using DataTransferObjects.Transfer;
using Entities.Exceptions;
using Entities.Models;

namespace Service;

public class FeedbackService(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger) : IFeedbackService
{
    private IFeedbackRepository _repo => repositoryManager.Feedback;

    public async Task<Feedback> CreateFeedbackAsync(FeedbackCreationDTO feedbackCreation)
    {
        if (feedbackCreation == null) throw new FeedbackForCreationBadRequestException();//TODO: Create this exception

        Feedback feedback = mapper.Map<FeedbackCreationDTO, Feedback>(feedbackCreation);

        return await _repo.CreateFeedbackAsync(feedback);
    }

    public async Task DeleteFeedbackByIdAsync(Guid id)
    {
        Feedback? feedback = await _repo.GetFeedbackByIdAsync(id);
        if (feedback == null) throw new FeedbackDeleteByIdNotFoundException(id);

        await _repo.DeleteFeedbackAsync(feedback);
    }

    public async Task<FeedbackDTO> GetFeedbackByIdAsync(Guid id)
    {
        return mapper.Map<Feedback, FeedbackDTO>(await _repo.GetFeedbackByIdAsync(id));
    }

    public async Task<IEnumerable<FeedbackDTO>> GetFeedbacksAsync()
    {
        IEnumerable<Feedback> feedbacks = await _repo.GetAllFeedbacksAsync();

        return mapper.Map<IEnumerable<Feedback>, IEnumerable<FeedbackDTO>>(feedbacks);
    }
}
