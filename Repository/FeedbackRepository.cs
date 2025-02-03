using Contracts.Repositories;
using Entities.Models;
using Redis.OM.Contracts;

namespace Repository;

public class FeedbackRepository(IRedisConnectionProvider provider) : RepositoryBase<Feedback>(provider), IFeedbackRepository
{
    public async Task<Feedback> CreateFeedbackAsync(Feedback feedback)
    {
        await CreateAsync(feedback);
        return feedback;
    }

    public async Task DeleteFeedbackAsync(Feedback feedback)
    {
        await DeleteAsync(feedback);
    }

    public async Task<List<Feedback>> GetAllFeedbacksAsync()
    {
        return [.. await FindAllAsync()];
    }

    public async Task<Feedback> GetFeedbackByIdAsync(Guid id)
    {
        return (await FindByConditionAsync(t => t.Id == id)).FirstOrDefault()!;
    }
}