using Contracts.Repositories;
using Redis.OM.Contracts;
using Repository;

namespace Repository;

public class RepositoryManager(IRedisConnectionProvider provider) : IRepositoryManager
{
    private readonly IRedisConnectionProvider _provider = provider;

    private readonly Lazy<IQTITestRepository> _qtiTestRepository = new Lazy<IQTITestRepository>(() => new QTITestRepository(provider));
    private readonly Lazy<IFeedbackRepository> _feedbackRepository = new Lazy<IFeedbackRepository>(() => new FeedbackRepository(provider));

    public IQTITestRepository QTITest => _qtiTestRepository.Value;
    public IFeedbackRepository Feedback => _feedbackRepository.Value;

    public Task SaveAsync()
    {
        return Task.CompletedTask;
    }
}
